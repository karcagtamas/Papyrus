using KarcagS.Blazor.Common.Components.Dialogs;
using KarcagS.Blazor.Common.Enums;
using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using Papyrus.Client.Shared.Components.Common.Editor;
using Papyrus.Client.Shared.Dialogs.Notes;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.HubEvents;
using System.Text;

namespace Papyrus.Client.Pages.Notes;

public partial class NoteEditor : ComponentBase, IDisposable
{
    [Parameter]
    public string Id { get; set; } = default!;

    private Editor? Editor = new();

    private HubConnection? hub;
    private string PageTitle { get; set; } = "Editor [New Document]";
    private NoteDTO? Note { get; set; }
    private NoteRightsDTO NoteRights { get; set; } = new();
    private string ClientId { get; set; } = string.Empty;
    private List<UserLightDTO> Users { get; set; } = new();
    private bool DataCollapsed { get; set; } = true;
    private bool IsSaving { get; set; } = false;
    private bool PageLoaded { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        PageTitle = L["Title", L["DefaultTitle"]];
        DataCollapsed = true;
        ClientId = await TokenService.GetClientId();

        await Refresh();

        InitHub();

        await InvokeAsync(StateHasChanged);
    }

    private async Task Refresh()
    {
        PageLoaded = false;
        var rights = await NoteService.GetRights(Id);

        if (!rights.CanView)
        {
            Navigation.NavigateTo("/dashboard");
            return;
        }

        NoteRights = rights;
        PageLoaded = true;

        var note = await NoteService.Get<NoteDTO>(Id);

        if (ObjectHelper.IsNotNull(note))
        {
            Note = note;

            UpdatePageTitle();

            Users = new();
            var users = await EditorService.GetMembers(Note.Id);
            Users.AddRange(users);

            await InvokeAsync(StateHasChanged);

            if (ObjectHelper.IsNotNull(Editor))
            {
                await Editor.SetValue(note.Content);
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    private void InitHub()
    {
        hub = new HubConnectionBuilder()
            .WithUrl($"{ApplicationSettings.BaseUrl}/editor", options =>
            {
                options.AccessTokenProvider = () => TokenService.GetAccessTokenProvider();
            })
            .Build();

        hub?.On<byte[]>(EditorHubEvents.EditorChanged, ApplyDiffs);

        hub?.On<UserLightDTO>(EditorHubEvents.EditorMemberJoined, async (user) =>
        {
            if (ObjectHelper.IsNotNull(user))
            {
                Users.RemoveAll(x => x.Id == user.Id);
                Users.Add(user);
                await InvokeAsync(StateHasChanged);
            }
        });

        hub?.On<string>(EditorHubEvents.EditorMemberLeft, async (user) =>
        {
            // Remove user from the list
            Users.RemoveAll(x => x.Id == user);
            await InvokeAsync(StateHasChanged);
        });

        hub?.On<NoteChangeEventArgs>(EditorHubEvents.EditorNoteUpdated, async (args) =>
        {
            if (ObjectHelper.IsNotNull(Note))
            {
                Note.Title = args.Title;
                Note.Tags = args.Tags;
                Note.Public = args.Public;
                UpdatePageTitle();
                await InvokeAsync(StateHasChanged);
            }
        });

        hub?.On(EditorHubEvents.EditorNoteDeleted, () => HandleDeleteAction());

        ObjectHelper.WhenNotNull(hub, async (h) =>
        {
            await h.StartAsync();
            await h.SendAsync(EditorHubEvents.EditorConnect, Id);
        });
    }

    private void SaveDirtyState() => ObjectHelper.WhenNotNull(Editor, async e => await e.SaveDirtyState());

    private void HandleChange(string content)
    {
        ObjectHelper.WhenNotNull(hub, (h) =>
        {
            if (h.State != HubConnectionState.Connected)
            {
                Toaster.Open(new ToasterSettings { Caption = L["Hub.Message.Disconnected"], Type = ToasterType.Error });
                return;
            }

            ObjectHelper.WhenNotNull(Editor, async (e) =>
            {
                // Pre-save actions
                IsSaving = true;
                await InvokeAsync(StateHasChanged);

                // Save
                await h.SendAsync(EditorHubEvents.EditorShare, Id, Encoding.UTF8.GetBytes(content));

                // Post-save actions
                e.IsDirty = false;
                IsSaving = false;
                await InvokeAsync(StateHasChanged);
            });
        });
    }

    private async Task OpenEdit()
    {
        if (!NoteRights.CanEdit)
        {
            return;
        }

        var parameters = new DialogParameters { { "NoteId", Id }, { "Rights", NoteRights } };

        await Helper.OpenEditorDialog<NoteEditDialog>("Edit Note", async (res) =>
        {
            if (res.Performed)
            {
                if (res.Event == EditorCloseEvent.Remove)
                {
                    hub?.SendAsync(EditorHubEvents.EditorNoteDeleted, Id);

                    // Will notify other clients
                    HandleDeleteAction();
                }
                else if (res.Event == EditorCloseEvent.Edit)
                {
                    // Fetch new data
                    var note = await NoteService.GetLight(Id);

                    ObjectHelper.WhenNotNull(note, async n =>
                    {
                        if ((hub?.State ?? HubConnectionState.Disconnected) != HubConnectionState.Connected)
                        {
                            Toaster.Open(new ToasterSettings { Caption = L["Hub.Message.Disconnected"], Type = ToasterType.Error });
                            return;
                        }

                        // Notify other clients
                        hub?.SendAsync(EditorHubEvents.EditorUpdateNote, Id, new NoteChangeEventArgs { Title = n.Title, Tags = n.Tags, Public = n.Public });

                        if (ObjectHelper.IsNotNull(Note))
                        {
                            // Internal update
                            Note.Title = n.Title;
                            Note.Tags = n.Tags;
                            Note.Public = n.Public;
                        }
                        UpdatePageTitle();
                        await InvokeAsync(StateHasChanged);
                    });
                }
            }
        }, parameters);
    }

    private async Task OpenLogs()
    {
        if (!NoteRights.CanViewLogs)
        {
            return;
        }

        var parameters = new DialogParameters { { "NoteId", Id } };

        var options = new DialogOptions { FullScreen = true };

        await Helper.OpenDialog<NoteActionLogDialog>("Note Action Log", () => { }, parameters, options);
    }

    private async Task ApplyDiffs(byte[] content)
    {
        var stringContent = Encoding.UTF8.GetString(content);
        if (ObjectHelper.IsNotNull(Editor) && ObjectHelper.IsNotNull(Note))
        {
            await Editor.SetValue(stringContent);
            Note.Content = stringContent;
        }

        await InvokeAsync(StateHasChanged);
    }

    private void UpdatePageTitle()
    {
        PageTitle = L["Title", Note?.Title ?? L["DefaultTitle"]];
    }

    private void HandleDeleteAction()
    {
        var url = ObjectHelper.IsNotNull(Note) && ObjectHelper.IsNotNull(Note.GroupId) ? $"/groups/{Note.GroupId}/notes" : "/notes";
        Navigation.NavigateTo(url);
    }

    public async void Dispose()
    {
        if (ObjectHelper.IsNotNull(hub))
        {
            await hub.SendAsync(EditorHubEvents.EditorDisconnect, Id);
            await hub.DisposeAsync();
        }
    }

}
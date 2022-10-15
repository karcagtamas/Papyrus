using System.Text;
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

namespace Papyrus.Client.Pages.Notes;

public partial class NoteEditor : ComponentBase, IDisposable
{
    [Parameter]
    public string Id { get; set; } = default!;

    private Editor? Editor = new();

    private HubConnection? editorHub;
    private HubConnection? noteHub;
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

        InitHubs();

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

    private void InitHubs()
    {
        editorHub = new HubConnectionBuilder()
            .WithUrl($"{ApplicationSettings.BaseUrl}/editor?Editor={Id}", options =>
            {
                options.AccessTokenProvider = () => TokenService.GetAccessTokenProvider();
            })
            .Build();

        editorHub?.On<byte[]>(EditorHubEvents.EditorChanged, ApplyDiffs);

        editorHub?.On<UserLightDTO>(EditorHubEvents.EditorMemberJoined, async (user) =>
        {
            if (ObjectHelper.IsNotNull(user))
            {
                Users.RemoveAll(x => x.Id == user.Id);
                Users.Add(user);
                await InvokeAsync(StateHasChanged);
            }
        });

        editorHub?.On<string>(EditorHubEvents.EditorMemberLeft, async (user) =>
        {
            // Remove user from the list
            Users.RemoveAll(x => x.Id == user);
            await InvokeAsync(StateHasChanged);
        });

        noteHub = new HubConnectionBuilder()
            .WithUrl($"{ApplicationSettings.BaseUrl}/note?Note={Id}", options =>
            {
                options.AccessTokenProvider = () => TokenService.GetAccessTokenProvider();
            })
            .Build();

        noteHub?.On<NoteLightDTO>(NoteHubEvents.NoteUpdated, async (data) =>
        {
            if (ObjectHelper.IsNotNull(Note))
            {
                Note.Title = data.Title;
                Note.Tags = data.Tags;
                Note.Public = data.Public;
                Note.Archived = data.Archived;
                Note.LastUpdate = data.LastUpdate;
                Note.LastUpdater = data.LastUpdater;
                UpdatePageTitle();
                await InvokeAsync(StateHasChanged);
            }
        });

        noteHub?.On(NoteHubEvents.NoteDeleted, () => HandleDeleteAction());

        ObjectHelper.WhenNotNull(editorHub, async (h) =>
        {
            try
            {
                await h.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                editorHub = null;
            }
        });
        ObjectHelper.WhenNotNull(noteHub, async (h) =>
        {
            try
            {
                await h.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                noteHub = null;
            }
        });
    }

    private void SaveDirtyState() => ObjectHelper.WhenNotNull(Editor, async e => await e.SaveDirtyState());

    private void HandleChange(string content)
    {
        ObjectHelper.WhenNotNull(editorHub, (h) =>
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

        var parameters = new DialogParameters { { "NoteId", Id }, { "DeleteEnabled", NoteRights.CanDelete } };

        await Helper.OpenEditorDialog<NoteEditDialog>("Edit Note", (res) =>
        {
            // Socket Response will refresh
            if (res.Performed)
            {
                if (res.Event == EditorCloseEvent.Remove)
                {
                }
                else if (res.Event == EditorCloseEvent.Edit)
                {
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

    public void Dispose()
    {
        ObjectHelper.WhenNotNull(editorHub, async h => await h.DisposeAsync());
        ObjectHelper.WhenNotNull(noteHub, async h => await h.DisposeAsync());
    }

}

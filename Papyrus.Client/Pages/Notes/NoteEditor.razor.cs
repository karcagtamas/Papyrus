using KarcagS.Blazor.Common.Components.Dialogs;
using KarcagS.Blazor.Common.Enums;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using Papyrus.Client.Services.Auth.Interfaces;
using Papyrus.Client.Services.Editor.Interfaces;
using Papyrus.Client.Services.Notes.Interfaces;
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

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ITokenService TokenService { get; set; } = default!;

    [Inject]
    private INoteService NoteService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    [Inject]
    private IEditorService EditorService { get; set; } = default!;

    [Inject]
    private IToasterService Toaster { get; set; } = default!;

    private Editor? Editor = new();

    private HubConnection? hub;
    private string PageTitle { get; set; } = "Editor [New Document]";
    private NoteDTO Note { get; set; } = default!;
    private string ClientId { get; set; } = string.Empty;
    private List<UserLightDTO> Users { get; set; } = new();
    private bool DataCollapsed { get; set; } = true;

    protected override async void OnInitialized()
    {
        await Refresh();

        DataCollapsed = true;

        ClientId = await TokenService.GetClientId();

        InitHub();

        base.OnInitialized();
        await InvokeAsync(StateHasChanged);
    }

    private async Task Refresh()
    {
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

            await InvokeAsync(StateHasChanged);
        }
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
            Note.Title = args.Title;
            Note.Tags = args.Tags;
            UpdatePageTitle();
            await InvokeAsync(StateHasChanged);
        });

        ObjectHelper.WhenNotNull(hub, async (h) =>
        {
            await h.StartAsync();
            await h.SendAsync(EditorHubEvents.EditorConnect, Id);
        });
    }

    private void HandleChange(string content)
    {
        ObjectHelper.WhenNotNull(hub, (h) =>
        {
            if (h.State != HubConnectionState.Connected)
            {
                Toaster.Open(new ToasterSettings { Caption = "Connection unexpectedly disconnected. Please try to reload the page.", Type = ToasterType.Error });
                return;
            }

            ObjectHelper.WhenNotNull(Editor, async (e) =>
            {
                await h.SendAsync(EditorHubEvents.EditorShare, Id, Encoding.UTF8.GetBytes(content));
            });
        });
    }

    private async Task OpenEdit()
    {
        var parameters = new DialogParameters { { "NoteId", Id } };

        await HelperService.OpenEditorDialog<NoteEditDialog>("Edit Note", async (res) =>
        {
            if (res.Performed)
            {
                if (res.Event == EditorCloseEvent.Remove)
                {
                    var url = ObjectHelper.IsNotNull(Note.GroupId) ? $"/groups/{Note.GroupId}/notes" : "/notes";
                    NavigationManager.NavigateTo(url);
                }
                else if (res.Event == EditorCloseEvent.Edit)
                {
                    var note = await NoteService.GetLight(Id);

                    ObjectHelper.WhenNotNull(note, async n =>
                    {
                        if ((hub?.State ?? HubConnectionState.Disconnected) != HubConnectionState.Connected)
                        {
                            Toaster.Open(new ToasterSettings { Caption = "Connection unexpectedly disconnected. Please try to reload the page.", Type = ToasterType.Error });
                            return;
                        }

                        hub?.SendAsync(EditorHubEvents.EditorUpdateNote, Id, new NoteChangeEventArgs { Title = n.Title, Tags = n.Tags });

                        Note.Title = n.Title;
                        Note.Tags = n.Tags;
                        UpdatePageTitle();
                        await InvokeAsync(StateHasChanged);
                    });
                }
            }
        }, parameters);
    }

    private async Task ApplyDiffs(byte[] content)
    {
        var stringContent = Encoding.UTF8.GetString(content);
        if (ObjectHelper.IsNotNull(Editor))
        {
            await Editor.SetValue(stringContent);
            Note.Content = stringContent;
        }

        await InvokeAsync(StateHasChanged);
    }

    private void UpdatePageTitle()
    {
        PageTitle = $"Editor [{Note.Title}]";
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
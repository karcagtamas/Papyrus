using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using Papyrus.Client.Services.Auth.Interfaces;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Client.Shared.Components.Common.Editor;
using Papyrus.Client.Shared.Dialogs.Notes;
using Papyrus.Shared.DiffMatchPatch;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.HubEvents;

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
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

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

            PageTitle = $"Editor [{Note.Title}]";

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

        hub?.On<List<TransportDiff>>(EditorHubEvents.EditorChanged, async (diffs) =>
        {
            await ApplyDiffs(diffs);
        });

        hub?.On<string, EditorMemberChange>(EditorHubEvents.EditorMemberChanged, async (user, action) =>
        {
            if (action == EditorMemberChange.Join)
            {
                var dto = await UserService.Light(user);

                if (dto is not null)
                {
                    Users.RemoveAll(x => x.Id == user);
                    Users.Add(dto);
                    await InvokeAsync(StateHasChanged);
                }
            }
            else if (action == EditorMemberChange.Leave)
            {
                Users.RemoveAll(x => x.Id == user);
                await InvokeAsync(StateHasChanged);
            }
        });

        ObjectHelper.WhenNotNull(hub, async (h) =>
        {
            await h.StartAsync();
            await h.SendAsync(EditorHubEvents.EditorConnect, Id);
        });
    }

    private void HandleChange(string content)
    {
        ObjectHelper.WhenNotNull(hub, async (h) =>
        {
            var diffs = new DiffMatchPatch().DiffMain(ObjectHelper.OrElse(Editor?.GetValue(), ""), content);
            if (diffs is not null)
            {
                await h.SendAsync(EditorHubEvents.EditorShare, Id, diffs.Select(diff => new TransportDiff(diff)).ToList());
            }
        });
    }

    private async Task OpenEdit() 
    {
        var parameters = new DialogParameters { { "NoteId", Id } };

        // TODO: Check EDIT or DELETE event
        // TODO: Refresh by socket
        await HelperService.OpenDialog<NoteEditDialog>("Edit Note", async () => await Refresh(), parameters);
    }

    private async Task ApplyDiffs(List<TransportDiff> tDiffs)
    {
        var diffs = tDiffs.Select(diff => new Diff(diff)).ToList();
        var patches = new DiffMatchPatch().PatchMake(diffs);
        var patched = new DiffMatchPatch().PatchApply(patches, ObjectHelper.OrElse(Editor?.GetValue(), ""));
        var patchResults = (bool[])patched[1];

        if (patchResults.Length == patches.Count && patchResults.All(x => x))
        {
            string result = (string)patched[0];

            if (ObjectHelper.IsNotNull(Editor))
            {
                await Editor.SetValue(result);
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    public async void Dispose()
    {
        if (hub is not null)
        {
            await hub.SendAsync(EditorHubEvents.EditorDisconnect, Id);
            await hub.DisposeAsync();
        }
    }

}
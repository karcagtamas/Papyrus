using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DiffMatchPatch;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.HubEvents;

namespace Papyrus.Client.Pages.Editor;

public partial class Editor : ComponentBase, IDisposable
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    [Inject]
    private ITokenService TokenService { get; set; }

    [Inject]
    private IUserService UserService { get; set; }

    private bool ColorPickerOpened { get; set; }
    private EditorAction? ColorAction { get; set; }
    private MudColor SelectedColor { get; set; } = new MudColor("#FF1212FF");
    private string Content { get; set; } = "";

    private List<UserLightDTO> Users { get; set; } = new List<UserLightDTO>();

    private HubConnection? hub;
    private ElementReference? editorReference;
    private string clientId = "";


    protected override async Task OnInitializedAsync()
    {
        clientId = await TokenService.GetClientId();
        string? token = await TokenService.GetAccessToken();
        hub = new HubConnectionBuilder()
            .WithUrl($"{ApplicationSettings.BaseUrl}/editor", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(token);
            })
            .Build();

        hub?.On<List<TransportDiff>>(EditorHubEvents.EditorChanged, async (diffs) =>
        {
            await ApplyDiffs(diffs);
        });

        hub?.On<string, EditorMemberChange>(EditorHubEvents.EditorMemberChange, async (user, action) =>
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


        if (hub is not null)
        {
            await hub.StartAsync();
            await hub.SendAsync(EditorHubEvents.EditorConnect, "ALMA");
        }
    }

    private async Task ApplyDiffs(List<TransportDiff> tDiffs)
    {
        var diffs = tDiffs.Select(diff => new Diff(diff)).ToList();
        var patches = new DiffMatchPatch().PatchMake(diffs);
        var patched = new DiffMatchPatch().PatchApply(patches, Content);
        var patchResults = (bool[])patched[1];

        if (patchResults.Length == patches.Count && patchResults.All(x => x))
        {
            string result = (string)patched[0];
            Content = result;
            Console.WriteLine($"Content changed to:\n {Content}");
            await SetEditorValue(Content, clientId);
            await InvokeAsync(StateHasChanged);
        }
    }

    private async void ExecuteAction(EditorAction action, string param = "")
    {
        switch (action)
        {
            case EditorAction.Bold:
                await JSRuntime.InvokeVoidAsync("document.execCommand", "bold");
                break;
            case EditorAction.Italic:
                await JSRuntime.InvokeVoidAsync("document.execCommand", "italic");
                break;
            case EditorAction.Underline:
                await JSRuntime.InvokeVoidAsync("document.execCommand", "underline");
                break;
            case EditorAction.Strikethrough:
                await JSRuntime.InvokeVoidAsync("document.execCommand", "strikethrough");
                break;
            case EditorAction.TextColor:
                await JSRuntime.InvokeVoidAsync("document.execCommand", "forecolor", false, param);
                break;
            case EditorAction.BackColor:
                await JSRuntime.InvokeVoidAsync("document.execCommand", "backcolor", false, param);
                break;
            default: return;
        }
    }

    private async Task ContentChanged(ChangeEventArgs args)
    {
        var old = Content;
        var current = await GetEditorValue();

        // Current cursor
        current = current.Replace("[{CURRENT_CURSOR}]", "<span class='editor-cursor' id='" + clientId + "'></span>");

        if (hub is not null)
        {
            var diffs = new DiffMatchPatch().DiffMain(old, current);
            if (diffs is not null)
            {
                await hub.SendAsync(EditorHubEvents.EditorShare, "ALMA", diffs.Select(diff => new TransportDiff(diff)).ToList());
            }
        }
    }

    private async Task<string> GetEditorValue()
    {
        return await JSRuntime.InvokeAsync<string>("getEditorValueByReference", editorReference);
    }

    private async Task SetEditorValue(string value, string clientId)
    {
        await JSRuntime.InvokeVoidAsync("setEditorValueByReference", editorReference, value, clientId);
    }

    private void ChooseColor(EditorAction action)
    {
        ColorPickerOpened = true;
        ColorAction = action;
    }

    private void CloseColorChooser(bool cancel)
    {
        if (!cancel && SelectedColor is not null && ColorAction is not null)
        {
            ExecuteAction(ColorAction ?? EditorAction.None, SelectedColor.ToString());
        }

        ColorPickerOpened = false;
        ColorAction = null;
    }

    public async void Dispose()
    {
        if (hub is not null)
        {
            await hub.SendAsync(EditorHubEvents.EditorDisconnect, "ALMA");
            await hub.DisposeAsync();
        }
    }
}
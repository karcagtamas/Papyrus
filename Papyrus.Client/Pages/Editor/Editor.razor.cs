using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using Papyrus.Shared.HubEvents;

namespace Papyrus.Client.Pages.Editor;

public partial class Editor : ComponentBase, IDisposable
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    private bool ColorPickerOpened { get; set; }
    private EditorAction? ColorAction { get; set; }
    private MudColor SelectedColor { get; set; } = new MudColor("#FF1212FF");
    private string Content { get; set; } = "";

    private HubConnection? hub;
    private ElementReference? editorReference;

    private List<EditorDelta> Deltas = new();


    protected override async Task OnInitializedAsync()
    {
        hub = new HubConnectionBuilder()
            .WithUrl($"{ApplicationSettings.BaseUrl}/editor")
            .Build();

        hub?.On<string>(EditorHubEvents.EditorChanged, async (content) =>
        {
            Console.WriteLine($"Content changed to:\n {content}");
            Content = content;
            await SetEditorValue(content);
            await InvokeAsync(StateHasChanged);
        });

        if (hub is not null)
        {
            await hub.StartAsync();
        }

        await StartObserve();
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

    private async void ContentChanged(ChangeEventArgs args)
    {
        Content = await GetEditorValue();
        if (hub is not null)
        {
            await hub.SendAsync(EditorHubEvents.EditorChange, Content);
        }
    }

    private async Task StartObserve()
    {
        await JSRuntime.InvokeVoidAsync("startEditorObserve", editorReference);
    }

    private async Task<string> GetEditorValue()
    {
        return await JSRuntime.InvokeAsync<string>("getEditorValueByReference", editorReference);
    }

    private async Task SetEditorValue(string value)
    {
        await JSRuntime.InvokeVoidAsync("setEditorValueByReference", editorReference, value);
    }

    private async Task<int> GetCursorPosition()
    {
        return await JSRuntime.InvokeAsync<int>("getCursorPositon", editorReference);
    }

    private async Task EditorClick(MouseEventArgs args)
    {
        Deltas.Add(new EditorDelta
        {
            Action = EditorDeltaAction.Click,
            Context = new ClickContext
            {
                CursorPosition = await GetCursorPosition()
            }
        });
        Deltas.ForEach(x => Console.WriteLine(x));
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
            await hub.DisposeAsync();
        }
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using Papyrus.Shared.DiffMatchPatch;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Papyrus.Client.Shared.Components.Common.Editor;

public partial class Editor : ComponentBase, IDisposable
{
    [Parameter, EditorRequired]
    public string ClientId { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnContentChange { get; set; }

    [Parameter]
    public RenderFragment RightToolbar { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private bool ColorPickerOpened { get; set; }
    private EditorAction? ColorAction { get; set; }
    private MudColor SelectedColor { get; set; } = new MudColor("#FF1212FF");
    private string Content { get; set; } = "";
    private ElementReference? editorReference;

    private Subject<string> subject = new();
    private IDisposable? disposable;

    protected override void OnInitialized()
    {
        disposable = subject.Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(async (e) => await OnContentChange.InvokeAsync(e));
        base.OnInitialized();
    }

    public async Task SetValue(string content)
    {
        var current = await GetEditorValue(true);

        Content = current;

        var diffs = new DiffMatchPatch().DiffMain(Content, content);

        diffs.Where(x => x.Operation == Operation.DELETE && x.Text == "‎")
            .ToList()
            .ForEach(x => x.Operation = Operation.EQUAL);

        if (diffs.Count > 0)
        {
            Diff.ApplyDiffs(Content, diffs, async (res) =>
            {
                Content = res;
                await SetEditorValue(Content, ClientId);
                Console.WriteLine($"Content changed to:\n {Content}");
            });
        }
    }

    public string GetValue() => Content;

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

    private async Task ContentChanged(ChangeEventArgs args) => subject.OnNext(await GetEditorValue());

    private async Task<string> GetEditorValue(bool withCursorLocation = false) => await JSRuntime.InvokeAsync<string>("getEditorValueByReference", editorReference, withCursorLocation);

    private async Task SetEditorValue(string value, string clientId) => await JSRuntime.InvokeVoidAsync("setEditorValueByReference", editorReference, value, clientId);

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

    public void Dispose() => disposable?.Dispose();
}
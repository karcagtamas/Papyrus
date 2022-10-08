using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using Papyrus.Client.Utils;
using Papyrus.Shared.DiffMatchPatch;

namespace Papyrus.Client.Shared.Components.Common.Editor;

public partial class Editor : ComponentBase, IDisposable
{
    [Parameter, EditorRequired]
    public string ClientId { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnContentChange { get; set; }

    [Parameter]
    public RenderFragment RightToolbar { get; set; } = default!;

    [Parameter]
    public bool Disabled { get; set; } = false;
    private string Content { get; set; } = "";
    private ElementReference? editorReference;

    private Subject<string> subject = new();
    private IDisposable? disposable;

    public bool IsDirty { get; set; }

    public MudColor? LastColor { get; set; }

    protected override void OnInitialized()
    {
        disposable = subject.ThrottleMax(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(800))
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

                if (!Content.Contains("‎"))
                {
                    Content += "‎";
                }

                await SetEditorValue(Content, ClientId);
                Console.WriteLine($"Content changed to:\n{Content}");
            });
        }
    }

    public string GetValue() => Content;

    public async Task SaveDirtyState() => await OnContentChange.InvokeAsync(Content);

    private async void ExecuteAction(EditorAction action, string param = "")
    {
        if (Disabled)
        {
            return;
        }

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
        if (Disabled)
        {
            return;
        }

        IsDirty = true;
        subject.OnNext(await GetEditorValue());
    }

    private async Task<string> GetEditorValue(bool withCursorLocation = false) => await JSRuntime.InvokeAsync<string>("getEditorValueByReference", editorReference, withCursorLocation);

    private async Task SetEditorValue(string value, string clientId) => await JSRuntime.InvokeVoidAsync("setEditorValueByReference", editorReference, value, clientId);

    private async Task ChooseColor(EditorAction action)
    {
        if (Disabled)
        {
            return;
        }

        var color = await CommonService.OpenColorPickerDialog(LastColor);

        ObjectHelper.WhenNotNull(color, c =>
        {
            LastColor = c;
            ExecuteAction(action, c.ToString());
        });
    }

    public void Dispose() => disposable?.Dispose();
}

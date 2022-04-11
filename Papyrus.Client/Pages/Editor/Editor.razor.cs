using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Papyrus.Client.Pages.Editor;

public partial class Editor : ComponentBase
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    protected override void OnInitialized()
    {

    }

    private async void ExecuteAction(EditorAction action)
    {
        switch (action)
        {
            case EditorAction.Bold:
                await JSRuntime.InvokeVoidAsync("document.execCommand", "bold");
                break;
            default: return;
        }
    }
}
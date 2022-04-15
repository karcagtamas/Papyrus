using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor.Utilities;

namespace Papyrus.Client.Pages.Editor;

public partial class Editor : ComponentBase, IDisposable
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    private bool ColorPickerOpened { get; set; }
    private EditorAction? ColorAction { get; set; }
    private MudColor SelectedColor { get; set; } = new MudColor("#FF1212FF");

    private HubConnection? hub;

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine($"{ApplicationSettings.BaseUrl}/editor");
        hub = new HubConnectionBuilder()
            .WithUrl($"{ApplicationSettings.BaseUrl}/editor")
            .Build();

        hub?.On<DateTime>("ReceiveTest", (time) => { Console.WriteLine($"Message received at {time}"); });

        if (hub is not null)
        {
            await hub.StartAsync();
        }
    }

    private async void ExecuteAction(EditorAction action, string param = "")
    {
        if (hub is not null)
        {
            await hub.SendAsync("SendTest");
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

    private void ChooseColor(EditorAction action)
    {
        ColorPickerOpened = true;
        ColorAction = action;
    }

    private void CloseColorChooser(bool cancel)
    {
        if (!cancel && SelectedColor is not null && ColorAction is not null)
        {
            Console.WriteLine(SelectedColor.ToString());
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
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Client.Pages.Notes;

public partial class Notes : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private INoteService NoteService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private List<NoteLightDTO> NoteList { get; set; } = new();
    private NotePublishType PublishType { get; set; } = NotePublishType.All;
    private bool ArchivedStatus { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Refresh();
    }

    private async Task Refresh()
    {
        NoteList = await NoteService.GetByUser(PublishType, ArchivedStatus);

        await InvokeAsync(StateHasChanged);
    }

    private async Task Create()
    {
        var result = await NoteService.CreateEmpty(null);

        if (ObjectHelper.IsNotNull(result))
        {
            await Refresh();
            await JSRuntime.InvokeAsync<object>("open", $"/notes/editor/{result.Id}", "_blank");
        }
    }

    private async Task HandlePublishTypeChange(NotePublishType publishType)
    {
        PublishType = publishType;
        await Refresh();
    }

    private async Task HandleArchivedStatusChange(bool status)
    {
        ArchivedStatus = status;
        await Refresh();
    }
}

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
    private NoteSearchType SearchType { get; set; } = NoteSearchType.All;

    protected override async void OnInitialized()
    {
        await Refresh();
        base.OnInitialized();
    }

    private async Task Refresh()
    {
        NoteList = await NoteService.GetByUser(SearchType);

        await InvokeAsync(StateHasChanged);
    }

    private async Task Create()
    {
        var result = await NoteService.CreateEmpty(null);

        if (ObjectHelper.IsNotNull(result))
        {
            await JSRuntime.InvokeAsync<object>("open", $"/notes/editor/{result.Id}", "_blank");
        }
    }

    private async Task HandleSearchTypeChange(NoteSearchType searchType)
    {
        SearchType = searchType;
        await Refresh();
    }
}

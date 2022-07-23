using Microsoft.AspNetCore.Components;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupNotes : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private INoteService NoteService { get; set; } = default!;

    private List<NoteLightDTO> Notes { get; set; } = new();

    protected override async void OnInitialized()
    {
        await Refresh();
        base.OnInitialized();
    }

    private async Task Refresh()
    {
        Notes = await NoteService.GetByGroup(GroupId);
    }

    private void Create()
    {

    }
}

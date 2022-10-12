using Microsoft.AspNetCore.Components;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Pages.Notes;

public partial class Notes : ComponentBase
{
    [Parameter]
    [SupplyParameterFromQuery]
    public string? Folder { get; set; }

    private Task<List<NoteLightDTO>> Fetcher(NoteFilterQueryModel query) => NoteService.GetFiltered(query, null);
}

using Microsoft.AspNetCore.Components;

namespace Papyrus.Client.Pages.Notes;

public partial class Notes : ComponentBase
{
    [Parameter]
    [SupplyParameterFromQuery]
    public string? Folder { get; set; }
}

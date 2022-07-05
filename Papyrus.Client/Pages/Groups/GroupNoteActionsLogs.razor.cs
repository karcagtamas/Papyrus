using Microsoft.AspNetCore.Components;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupNoteActionLogs : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }
}

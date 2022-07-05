using Microsoft.AspNetCore.Components;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupActionLogs : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }
}

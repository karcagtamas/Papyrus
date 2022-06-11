using KarcagS.Blazor.Common.Store;
using Microsoft.AspNetCore.Components;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupActionLogs : ComponentBase
{
    [Inject]
    private IStoreService Store { get; set; } = default!;

    [Parameter]
    public int GroupId { get; set; }
}

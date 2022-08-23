using Microsoft.AspNetCore.Components;
using Papyrus.Client.Services.Groups.Interfaces;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupActionLogs : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupActionLogTableService GroupActionLogTableService { get; set; } = default!;

    private Dictionary<string, object> ExtraParams { get; set; } = new();

    protected override void OnInitialized()
    {
        ExtraParams = new Dictionary<string, object>
        {
            { "groupId", GroupId }
        };
        base.OnInitialized();
    }
}

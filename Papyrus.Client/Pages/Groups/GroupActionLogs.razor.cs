using Microsoft.AspNetCore.Components;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupActionLogs : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    private Dictionary<string, object> TableParams { get; set; } = new();
    private bool PageEnabled { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!await SetPageStatus())
        {
            return;
        }

        TableParams = new Dictionary<string, object>
        {
            { "groupId", GroupId }
        };

        await InvokeAsync(StateHasChanged);
    }

    private async Task<bool> SetPageStatus()
    {
        var rights = await GroupService.GetPageRights(GroupId);

        if (!rights.LogPageEnabled)
        {
            GroupService.NavigateToBase(GroupId, !rights.DataPageEnabled);
            return false;
        }

        PageEnabled = true;
        return true;
    }
}

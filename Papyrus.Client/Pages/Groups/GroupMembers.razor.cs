using Microsoft.AspNetCore.Components;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupMembers : ComponentBase
{
    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Parameter]
    public int GroupId { get; set; }

    private List<GroupMemberDTO> Members { get; set; } = new();
    private bool Loading { get; set; } = true;

    protected override async void OnInitialized()
    {
        await GetRoles();
        base.OnInitialized();
    }

    private async Task GetRoles()
    {
        Loading = true;
        await InvokeAsync(StateHasChanged);
        Members = await GroupService.GetMembers(GroupId);
        Loading = false;
        await InvokeAsync(StateHasChanged);
    }
}

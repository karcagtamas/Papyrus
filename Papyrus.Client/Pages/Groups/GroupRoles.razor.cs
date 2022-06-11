using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupRoles : ComponentBase
{
    [Inject]
    private IGroupRoleService GroupRoleService { get; set; } = default!;

    [Parameter]
    public int GroupId { get; set; }

    private List<GroupRoleDTO> Roles { get; set; } = new();
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
        Roles = await GroupRoleService.GetGroupList(GroupId);
        Loading = false;
        await InvokeAsync(StateHasChanged);
    }

    private void Edit(TableRowClickEventArgs<GroupRoleDTO> e) 
    {
        Console.WriteLine(e.Item.Name);
    }
}

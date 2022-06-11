using Microsoft.AspNetCore.Components;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupData : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    private GroupDTO? Group { get; set; } = default!;

    protected override async void OnInitialized()
    {
        await GetGroup();
        await base.OnInitializedAsync();
    }

    private async Task GetGroup()
    {
        Group = await GroupService.Get<GroupDTO>(GroupId);
        await InvokeAsync(StateHasChanged);
    }
}

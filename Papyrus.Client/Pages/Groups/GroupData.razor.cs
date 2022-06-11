using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupData : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

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

    private async void Edit()
    {
        await OpenDialog(GroupId);
    }

    private async Task OpenDialog(int? groupId)
    {
        var parameters = new DialogParameters { { "GroupId", groupId } };
        var dialog = DialogService.Show<GroupEditDialog>(groupId is null ? "Create Group" : "Edit Group", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await GetGroup();
        }
    }
}

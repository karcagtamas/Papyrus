using KarcagS.Blazor.Common.Components.Confirm;
using KarcagS.Blazor.Common.Services;
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
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IConfirmService ConfirmService { get; set; } = default!;

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    private GroupDTO? Group { get; set; } = default!;
    private bool Closable { get; set; } = false;

    protected override async void OnInitialized()
    {
        await GetGroup();
        await base.OnInitializedAsync();
    }

    private async Task GetGroup()
    {
        Group = await GroupService.Get<GroupDTO>(GroupId);
        Closable = await GroupService.IsClosable(GroupId);
        await InvokeAsync(StateHasChanged);
    }

    private async void Edit()
    {
        await OpenDialog(GroupId);
    }

    private async Task OpenDialog(int? groupId)
    {
        var parameters = new DialogParameters { { "GroupId", groupId } };
        await HelperService.OpenDialog<GroupEditDialog>(groupId is null ? "Create Group" : "Edit Group", async () => await GetGroup(), parameters);
    }

    private async Task Close()
    {
        if (!Closable)
        {
            return;
        }

        await ConfirmService.Open(new ConfirmDialogInput { Name = "Group", ActionName = "close", ActionFunction = async () => await GroupService.Close(GroupId) }, "Confirm Close", () => Navigation.NavigateTo("/my-groups"));
    }
}

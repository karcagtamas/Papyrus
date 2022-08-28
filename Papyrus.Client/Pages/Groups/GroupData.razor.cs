using KarcagS.Blazor.Common.Components.Confirm;
using KarcagS.Blazor.Common.Services.Interfaces;
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
    private GroupRightsDTO Rights { get; set; } = new();

    protected override async void OnInitialized()
    {
        await GetGroup();
        await base.OnInitializedAsync();
    }

    private async Task GetGroup()
    {
        Group = await GroupService.Get<GroupDTO>(GroupId);
        Rights = await GroupService.GetRights(GroupId);
        await InvokeAsync(StateHasChanged);
    }

    private async void Edit()
    {
        await OpenDialog(GroupId);
    }

    private async Task OpenDialog(int? groupId)
    {
        var parameters = new DialogParameters { { "GroupId", groupId } };
        await HelperService.OpenEditorDialog<GroupEditDialog>(groupId is null ? "Create Group" : "Edit Group", async (res) => await GetGroup(), parameters);
    }

    private async Task Close() => await ExecuteAction(Rights.CanClose, L["CloseAction"], L["CloseActionName"], () => GroupService.Close(GroupId), async () => await GetGroup());

    private async Task Open() => await ExecuteAction(Rights.CanOpen, L["OpenAction"], L["OpenActionName"], () => GroupService.Open(GroupId), async () => await GetGroup());

    private async Task Remove() => await ExecuteAction(Rights.CanRemove, L["RemoveAction"], L["RemoveActionName"], () => GroupService.Remove(GroupId), () => Navigation.NavigateTo("/my-groups"));

    private async Task ExecuteAction(bool preCheck, string action, string actionName, Func<Task<bool>> performAction, Action postAction)
    {
        if (!preCheck)
        {
            return;
        }

        await ConfirmService.Open(new ConfirmDialogInput { Name = L["Entity"], ActionName = actionName.ToLower(), ActionFunction = async () => await performAction() }, L["ActionConfirmTitle", action], () => postAction());
    }
}

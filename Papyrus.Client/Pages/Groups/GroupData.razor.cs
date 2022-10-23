using KarcagS.Blazor.Common.Components.Confirm;
using KarcagS.Blazor.Common.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupData : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    private GroupDTO? Group { get; set; } = default!;
    private GroupRightsDTO Rights { get; set; } = new();
    private bool PageEnabled { get; set; } = false;
    private List<GroupNoteListDTO> Notes { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!await SetPageStatus())
        {
            return;
        }

        await Refresh();
    }

    private async Task<bool> SetPageStatus()
    {
        var rights = await GroupService.GetPageRights(GroupId);

        if (!rights.DataPageEnabled)
        {
            GroupService.NavigateToBase(GroupId, true);
            return false;
        }

        PageEnabled = true;
        return true;
    }

    private async Task Refresh()
    {
        Group = await GroupService.Get<GroupDTO>(GroupId);
        Rights = await GroupService.GetRights(GroupId);

        Notes = await GroupService.GetRecentEdits(GroupId);

        await InvokeAsync(StateHasChanged);
    }

    private async void Edit()
    {
        if (!Rights.CanEdit)
        {
            return;
        }

        await OpenDialog(GroupId);
    }

    private async Task OpenDialog(int? groupId)
    {
        var parameters = new DialogParameters { { "GroupId", groupId } };
        await HelperService.OpenEditorDialog<GroupEditDialog>(groupId is null ? "Create Group" : "Edit Group", async (res) => await Refresh(), parameters);
    }

    private async Task Close() => await ExecuteAction(Rights.CanClose, L["CloseTitle"], L["CloseMessage"], () => GroupService.Close(GroupId), async () => await Refresh());

    private async Task Open() => await ExecuteAction(Rights.CanOpen, L["OpenTitle"], L["OpenMessage"], () => GroupService.Open(GroupId), async () => await Refresh());

    private async Task Remove() => await ExecuteAction(Rights.CanRemove, L["RemoveTitle"], L["RemoveMessage"], () => GroupService.Remove(GroupId), () => Navigation.NavigateTo("/my-groups"));

    private async Task ExecuteAction(bool preCheck, string title, string message, Func<Task<bool>> performAction, Action postAction)
    {
        if (!preCheck)
        {
            return;
        }

        await ConfirmService.Open(new ConfirmDialogInput { Message = message, ActionFunction = async () => await performAction() }, title, () => postAction());
    }

    private async Task OpenNote(string id)
    {
        await CommonService.OpenNote(id);
    }
}

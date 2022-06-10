using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class MyGroups : ComponentBase
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    private List<GroupListDTO> Groups { get; set; } = new();

    protected override async void OnInitialized()
    {
        await Refresh();
        await base.OnInitializedAsync();
    }

    private async void Create()
    {
        await OpenDialog(null);
    }

    private async Task OpenDialog(int? groupId)
    {
        var parameters = new DialogParameters { { "GroupId", groupId } };
        var dialog = DialogService.Show<GroupEditDialog>(groupId is null ? "Create Group" : "Edit Group", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await Refresh();
        }
    }

    private async Task Refresh()
    {
        Groups = await GroupService.GetUserList();
        await InvokeAsync(StateHasChanged);
    }
}

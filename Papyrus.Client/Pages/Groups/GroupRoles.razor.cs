using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupRoles : ComponentBase
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IGroupRoleService GroupRoleService { get; set; } = default!;

    [Parameter]
    public int GroupId { get; set; }

    private List<GroupRoleDTO> Roles { get; set; } = new();
    private bool Loading { get; set; } = true;
    private List<GroupRoleSettingsItem> SettingsItems = new List<GroupRoleSettingsItem>
    {
        new()
        {
            ColumnTitle = "Group Edit",
            ValueGetter = (dto) => dto.GroupEdit
        },
        new()
        {
            ColumnTitle = "Group Close",
            ValueGetter = (dto) => dto.GroupClose
        },
        new()
        {
            ColumnTitle = "Listing Notes",
            ValueGetter = (dto) => dto.ReadNoteList
        },
        new()
        {
            ColumnTitle = "Read Note",
            ValueGetter = (dto) => dto.ReadNote
        },
        new()
        {
            ColumnTitle = "Create Note",
            ValueGetter = (dto) => dto.CreateNote
        },
        new()
        {
            ColumnTitle = "Delete Note",
            ValueGetter = (dto) => dto.DeleteNote
        },
        new()
        {
            ColumnTitle = "Edit Note",
            ValueGetter = (dto) => dto.EditNote
        },
        new()
        {
            ColumnTitle = "Read Members",
            ValueGetter = (dto) => dto.ReadMemberList
        },
        new()
        {
            ColumnTitle = "Edit Members",
            ValueGetter = (dto) => dto.EditMemberList
        },
        new()
        {
            ColumnTitle = "Read Roles",
            ValueGetter = (dto) => dto.ReadRoleList
        },
        new()
        {
            ColumnTitle = "Edit Roles",
            ValueGetter = (dto) => dto.EditRoleList
        },
        new()
        {
            ColumnTitle = "Read Action Logs",
            ValueGetter = (dto) => dto.ReadGroupActionLog
        },
        new()
        {
            ColumnTitle = "Read Note Action Logs",
            ValueGetter = (dto) => dto.ReadNoteActionLog
        },
        new()
        {
            ColumnTitle = "Read Tags",
            ValueGetter = (dto) => dto.ReadTagList
        },
        new()
        {
            ColumnTitle = "Edit Tags",
            ValueGetter = (dto) => dto.EditTagList
        }
    };

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

    private async Task Create()
    {
        await OpenDialog(null);
    }

    private async Task Edit(TableRowClickEventArgs<GroupRoleDTO> e)
    {
        if (!e.Item.ReadOnly)
        {
            await OpenDialog(e.Item.Id);
        }
    }

    private async Task OpenDialog(int? groupRoleId)
    {
        var parameters = new DialogParameters { { "GroupRoleId", groupRoleId }, { "GroupId", GroupId } };

        var dialog = DialogService.Show<GroupRoleEditDialog>(groupRoleId is null ? "Create Group Role" : "Edit Group Role", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await GetRoles();
        }
    }
}

public class GroupRoleSettingsItem
{
    public string ColumnTitle { get; set; } = default!;
    public Func<GroupRoleDTO, bool> ValueGetter { get; set; } = (dto) => false;
}

using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Blazor.Common.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Client.Shared.Dialogs.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Pages.Groups;

public partial class GroupRoles : ComponentBase
{
    [Parameter]
    public int GroupId { get; set; }

    [Inject]
    private IGroupService GroupService { get; set; } = default!;

    [Inject]
    private IGroupRoleService GroupRoleService { get; set; } = default!;

    [Inject]
    private IHelperService HelperService { get; set; } = default!;

    private ListTable<GroupRoleDTO, int>? ListTable { get; set; }
    private TableDataSource<GroupRoleDTO, int> DataSource { get; set; } = default!;
    private TableConfiguration<GroupRoleDTO, int> Config { get; set; } = default!;

    private GroupRoleRightsDTO Rights { get; set; } = new();

    protected override async void OnInitialized()
    {
        await Refresh(false);
        DataSource = new TableDataSource<GroupRoleDTO, int>((filter) => GroupRoleService.GetByGroup(GroupId));
        Config = TableConfiguration<GroupRoleDTO, int>.Build()
            .AddTitle("Management Roles")
            .AddColumn(
                new()
                {
                    Key = "name",
                    Title = "Name",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => obj.Name,
                    ColorGetter = (obj, i) => obj.ReadOnly ? Color.Error : Color.Secondary,
                }
            )
            .AddColumn(
                new()
                {
                    Key = "readonly",
                    Title = "Read Only",
                    TitleColor = Color.Primary,
                    ValueGetter = (obj) => obj.ReadOnly ? "Yes" : "No"
                }
            )
            .AddColumn(BuildColumn("group-edit", "Group Edit", (obj) => obj.GroupEdit))
            .AddColumn(BuildColumn("group-close", "Group Close", (obj) => obj.GroupClose))
            .AddColumn(BuildColumn("listing-notes", "Listing Notes", (obj) => obj.ReadNoteList))
            .AddColumn(BuildColumn("read-note", "Read Note", (obj) => obj.ReadNote))
            .AddColumn(BuildColumn("create-note", "Create Note", (obj) => obj.CreateNote))
            .AddColumn(BuildColumn("delete-note", "Delete Note", (obj) => obj.DeleteNote))
            .AddColumn(BuildColumn("edit-note", "Edit Note", (obj) => obj.EditNote))
            .AddColumn(BuildColumn("read-members", "Read Members", (obj) => obj.ReadMemberList))
            .AddColumn(BuildColumn("edit-members", "Edit Members", (obj) => obj.EditMemberList))
            .AddColumn(BuildColumn("read-roles", "Read Roles", (obj) => obj.ReadRoleList))
            .AddColumn(BuildColumn("edit-roles", "Edit Roles", (obj) => obj.EditRoleList))
            .AddColumn(BuildColumn("read-logs", "Read Logs", (obj) => obj.ReadGroupActionLog))
            .AddColumn(BuildColumn("read-note-logs", "Read Note Logs", (obj) => obj.ReadNoteActionLog))
            .AddColumn(BuildColumn("read-tags", "Read Tags", (obj) => obj.GroupEdit))
            .AddColumn(BuildColumn("edit-tags", "Edit Tags", (obj) => obj.GroupEdit))
            .AddFilter(TableFilterConfiguration.Build().IsTextFilterEnabled(true));
        Config.ClickDisableOn = (obj) => !Rights.CanEdit || obj.ReadOnly;
        await InvokeAsync(StateHasChanged);
        base.OnInitialized();
    }

    private async Task Refresh(bool tableRefresh = true)
    {
        Rights = await GroupService.GetRoleRights(GroupId);

        if (tableRefresh)
        {
            ListTable?.Refresh();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task Create()
    {
        if (!Rights.CanCreate)
        {
            return;
        }

        await OpenDialog(null);
    }

    private async Task OpenDialog(int? groupRoleId)
    {
        var parameters = new DialogParameters { { "GroupRoleId", groupRoleId }, { "GroupId", GroupId } };

        await HelperService.OpenEditorDialog<GroupRoleEditDialog>(groupRoleId is null ? "Create Group Role" : "Edit Group Role", async (res) => await Refresh(), parameters, new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }

    private static TableColumn<GroupRoleDTO, int> BuildColumn(string key, string title, Func<GroupRoleDTO, bool> getter)
    {
        return new()
        {
            Key = key,
            Title = title,
            TitleColor = Color.Secondary,
            ValueGetter = (obj) => getter(obj) ? "Yes" : "No",
            ColorGetter = (obj, i) => getter(obj) ? Color.Success : Color.Error
        };
    }

    private async Task RowClickHandler(RowItem<GroupRoleDTO, int> item) => await OpenDialog(item.Id);
}

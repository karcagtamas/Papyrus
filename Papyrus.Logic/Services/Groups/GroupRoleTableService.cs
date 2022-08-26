using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table.Enums;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared;

namespace Papyrus.Logic.Services.Groups;

public class GroupRoleTableService : TableService<GroupRole, int>, IGroupRoleTableService
{
    private readonly IGroupRoleService groupRoleService;

    public GroupRoleTableService(IGroupRoleService groupRoleService)
    {
        this.groupRoleService = groupRoleService;
        Initialize();
    }

    public override Configuration<GroupRole, int> BuildConfiguration()
    {
        // TODO: Edit right check for readonly state
        return Configuration<GroupRole, int>
            .Build("group-role-table")
            .SetTitle("Management Roles")
            .AddColumn(Column<GroupRole, int>.Build("name")
                .SetTitle("Name")
                .AddValueGetter(obj => obj.Name))
            .AddColumn(Column<GroupRole, int>.Build("readonly")
                .SetTitle("Read Only")
                .AddValueGetter(obj => obj.ReadOnly)
                .SetFormatter(ColumnFormatter.Logic, "Yes", "No")
                .SetAlignment(Alignment.Center)
                .SetWidth(40))
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
            .AddColumn(BuildColumn("read-tags", "Read Tags", (obj) => obj.ReadTagList))
            .AddColumn(BuildColumn("edit-tags", "Edit Tags", (obj) => obj.EditTagList))
            .DisableClickOn(obj => obj.ReadOnly)
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddTagProvider((obj, col) =>
            {
                if (col.Key == "name")
                {
                    return obj.ReadOnly ? Tags.ReadOnly : Tags.RoleName;
                }

                return "";
            })
            .AddTagProvider((obj, col) =>
            {
                return col.Key switch
                {
                    "group-edit" => GetTag(obj.GroupEdit),
                    "group-close" => GetTag(obj.GroupClose),
                    "listing-notes" => GetTag(obj.ReadNoteList),
                    "read-note" => GetTag(obj.ReadNote),
                    "create-note" => GetTag(obj.CreateNote),
                    "delete-note" => GetTag(obj.DeleteNote),
                    "edit-note" => GetTag(obj.EditNote),
                    "read-members" => GetTag(obj.ReadMemberList),
                    "edit-members" => GetTag(obj.EditMemberList),
                    "read-roles" => GetTag(obj.ReadRoleList),
                    "edit-roles" => GetTag(obj.EditRoleList),
                    "read-logs" => GetTag(obj.ReadGroupActionLog),
                    "read-note-logs" => GetTag(obj.ReadNoteActionLog),
                    "read-tags" => GetTag(obj.ReadTagList),
                    "edit-tags" => GetTag(obj.EditTagList),
                    _ => "",
                };
            }); // TODO: Table readonly => because of right
    }

    public override DataSource<GroupRole, int> BuildDataSource()
    {
        return ListTableDataSource<GroupRole, int>.Build((query) => groupRoleService.GetListAsQuery(x => x.GroupId == int.Parse(query.ExtraParams["groupId"].ToString() ?? "0")))
            .OrderBy(x => x.ReadOnly, OrderDirection.Descend)
            .ThenBy(x => x.Id)
            .ApplyOrdering()
            .SetEFFilteredEntries("Name");
    }

    public override Table<GroupRole, int> BuildTable()
    {
        return ListTableBuilder<GroupRole, int>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }

    private static Column<GroupRole, int> BuildColumn(string key, string title, Func<GroupRole, object> getter)
    {
        return Column<GroupRole, int>.Build(key)
            .SetTitle(title)
            .SetAlignment(Alignment.Center)
            .AddValueGetter(getter)
            .SetFormatter(ColumnFormatter.Logic, "Yes", "No")
            .SetWidth(40);
    }

    private static string GetTag(bool value) => value ? Tags.TrueValue : Tags.FalseValue;
}

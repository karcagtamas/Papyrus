using DocumentFormat.OpenXml.Drawing;
using KarcagS.Common.Tools.Services;
using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared;

namespace Papyrus.Logic.Services.Groups;

public class GroupRoleTableService : TableService<GroupRole, int>, IGroupRoleTableService
{
    private readonly IGroupRoleService groupRoleService;
    private readonly ITranslationService translationService;
    private readonly ILanguageService languageService;
    private readonly IAuthorizationService authorization;
    private readonly IUtilsService<string> utils;
    private readonly string TranslationSegment = "GroupRole";

    public GroupRoleTableService(IGroupRoleService groupRoleService, ITranslationService translationService, ILanguageService languageService, IAuthorizationService authorization, IUtilsService<string> utils)
    {
        this.groupRoleService = groupRoleService;
        this.translationService = translationService;
        this.languageService = languageService;
        this.authorization = authorization;
        this.utils = utils;
        Initialize();
    }

    public override Configuration<GroupRole, int> BuildConfiguration()
    {
        var current = languageService.GetUserLangOrDefault();
        return Configuration<GroupRole, int>
            .Build("group-role-table")
            .SetTitle("Management Roles", "Table.Title")
            .AddColumn(Column<GroupRole, int>.Build("name")
                .SetTitle("Name", "TableColumn.Name")
                .AddValueGetter(obj => obj.IsDefault ? translationService.GetValue(obj.Name, TranslationSegment, current) : obj.Name))
            .AddColumn(Column<GroupRole, int>.Build("readonly")
                .SetTitle("Read Only", "TableColumn.ReadOnly")
                .AddValueGetter(obj => obj.ReadOnly)
                .SetFormatter(ColumnFormatter.Logic, "TrueValue", "FalseValue")
                .SetAlignment(Alignment.Center)
                .SetWidth(40))
            .AddColumn(BuildColumn("group-edit", "Group Edit", (obj) => obj.GroupEdit, "TableColumn.GroupEdit"))
            .AddColumn(BuildColumn("listing-notes", "Listing Notes", (obj) => obj.ReadNoteList, "TableColumn.ReadNotes"))
            .AddColumn(BuildColumn("read-note", "Read Note", (obj) => obj.ReadNote, "TableColumn.ReadNote"))
            .AddColumn(BuildColumn("create-note", "Create Note", (obj) => obj.CreateNote, "TableColumn.CreateNote"))
            .AddColumn(BuildColumn("delete-note", "Delete Note", (obj) => obj.DeleteNote, "TableColumn.DeleteNote"))
            .AddColumn(BuildColumn("edit-note", "Edit Note", (obj) => obj.EditNote, "TableColumn.EditNote"))
            .AddColumn(BuildColumn("read-members", "Read Members", (obj) => obj.ReadMemberList, "TableColumn.ReadMembers"))
            .AddColumn(BuildColumn("edit-members", "Edit Members", (obj) => obj.EditMemberList, "TableColumn.EditMembers"))
            .AddColumn(BuildColumn("read-roles", "Read Roles", (obj) => obj.ReadRoleList, "TableColumn.ReadRoles"))
            .AddColumn(BuildColumn("edit-roles", "Edit Roles", (obj) => obj.EditRoleList, "TableColumn.EditRoles"))
            .AddColumn(BuildColumn("read-logs", "Read Logs", (obj) => obj.ReadGroupActionLog, "TableColumn.ReadLogs"))
            .AddColumn(BuildColumn("read-note-logs", "Read Note Logs", (obj) => obj.ReadNoteActionLog, "TableColumn.ReadNoteLogs"))
            .AddColumn(BuildColumn("read-tags", "Read Tags", (obj) => obj.ReadTagList, "TableColumn.ReadTags"))
            .AddColumn(BuildColumn("edit-tags", "Edit Tags", (obj) => obj.EditTagList, "TableColumn.EditTags"))
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
            });
    }

    public override DataSource<GroupRole, int> BuildDataSource()
    {
        return ListTableDataSource<GroupRole, int>.Build((query) => groupRoleService.GetListAsQuery(x => x.GroupId == ExtractGroupId(query)))
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

    public override Task<bool> Authorize(QueryModel query) => ReadCheck(ExtractGroupId(query));

    private async Task<bool> ReadCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(utils.GetRequiredUserPrincipal(), id, GroupPolicies.ReadGroupRoles.Requirements);

        return result.Succeeded;
    }

    private static int ExtractGroupId(QueryModel query) => int.Parse(query.ExtraParams["groupId"].ToString() ?? "0");

    private static Column<GroupRole, int> BuildColumn(string key, string title, Func<GroupRole, object> getter, string? resourceKey = null)
    {
        return Column<GroupRole, int>.Build(key)
            .SetTitle(title, resourceKey)
            .SetAlignment(Alignment.Center)
            .AddValueGetter(getter)
            .SetFormatter(ColumnFormatter.Logic, "TrueValue", "FalseValue")
            .SetWidth(40);
    }

    private static string GetTag(bool value) => value ? Tags.TrueValue : Tags.FalseValue;
}

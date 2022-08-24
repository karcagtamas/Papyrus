using KarcagS.Common.Tools.Services;
using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table.Enums;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Logic.Services.Groups;

public class GroupMemberTableService : TableService<GroupMember, int>, IGroupMemberTableService
{
    private readonly IUtilsService<string> utilsService;
    private readonly IGroupMemberService groupMemberService;

    public GroupMemberTableService(IUtilsService<string> utilsService, IGroupMemberService groupMemberService)
    {
        this.utilsService = utilsService;
        this.groupMemberService = groupMemberService;
    }

    public override Configuration<GroupMember, int> BuildConfiguration()
    {
        var userId = utilsService.GetRequiredCurrentUserId();
        return Configuration<GroupMember, int>
            .Build("group-member-table")
            .AddTitle("Group Members")
            .AddColumn(Column<GroupMember, int>.Build("user")
                .SetTitle("User")
                .AddValueGetter(obj => obj.User.UserName)
                .SetWidth(480))
            .AddColumn(Column<GroupMember, int>.Build("role")
                .SetTitle("Role")
                .AddValueGetter(obj => obj.Role.Name))
            .AddColumn(Column<GroupMember, int>.Build("added-by")
                .SetTitle("Added By")
                .AddValueGetter(obj => WriteHelper.WriteNullableField(obj.AddedBy?.UserName))
                .SetWidth(320))
            .AddColumn(Column<GroupMember, int>.Build("join")
                .SetTitle("Join")
                .AddValueGetter(obj => obj.Creation)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(180))
            .DisableClickOn(obj => userId == obj.UserId)
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddTagProvider((obj, col) => userId == obj.UserId ? "CURRENT_USER" : ""); // TODO: Table readonly => because of right
    }

    public override DataSource<GroupMember, int> BuildDataSource()
    {
        return ListTableDataSource<GroupMember, int>.Build((query) => groupMemberService.GetListAsQuery(x => x.GroupId == int.Parse(query.ExtraParams["groupId"].ToString() ?? "0")))
            .ApplyDefaultOrdering(x => x.RoleId)
            .SetEFFilteredEntries("User.UserName", "Role.Name", "AddedBy.UserName");
    }

    public override Table<GroupMember, int> BuildTable()
    {
        return ListTableBuilder<GroupMember, int>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }
}

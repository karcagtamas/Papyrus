using KarcagS.Common.Tools.Services;
using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared;

namespace Papyrus.Logic.Services.Groups;

public class GroupMemberTableService : TableService<GroupMember, int>, IGroupMemberTableService
{
    private readonly IUtilsService<string> utilsService;
    private readonly IGroupMemberService groupMemberService;
    private readonly IRightService rightService;

    public GroupMemberTableService(IUtilsService<string> utilsService, IGroupMemberService groupMemberService, IRightService rightService) : base()
    {
        this.utilsService = utilsService;
        this.groupMemberService = groupMemberService;
        this.rightService = rightService;
        Initialize();
    }

    public override Configuration<GroupMember, int> BuildConfiguration()
    {
        var userId = utilsService.GetRequiredCurrentUserId();
        return Configuration<GroupMember, int>
            .Build("group-member-table")
            .SetTitle("Group Members", "Table.Title")
            .AddColumn(Column<GroupMember, int>.Build("user")
                .SetTitle("User", "TableColumn.User")
                .AddValueGetter(obj => obj.User.UserName)
                .SetWidth(480))
            .AddColumn(Column<GroupMember, int>.Build("role")
                .SetTitle("Role", "TableColumn.Role")
                .AddValueGetter(obj => obj.Role.Name))
            .AddColumn(Column<GroupMember, int>.Build("added-by")
                .SetTitle("Added By", "TableColumn.AddedBy")
                .AddValueGetter(obj => WriteHelper.WriteNullableField(obj.AddedBy?.UserName))
                .SetWidth(320))
            .AddColumn(Column<GroupMember, int>.Build("join")
                .SetTitle("Join", "TableColumn.Join")
                .AddValueGetter(obj => obj.Creation)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(180))
            .DisableClickOn(obj => userId == obj.UserId || obj.Group.OwnerId == obj.UserId)
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddTagProvider((obj, col) => userId == obj.UserId ? Tags.CurrentUserTag : "");
    }

    public override DataSource<GroupMember, int> BuildDataSource()
    {
        return ListTableDataSource<GroupMember, int>.Build((query) =>
            groupMemberService.GetListAsQuery(x => x.GroupId == ExtractGroupId(query))
                .Include(x => x.User)
                .Include(x => x.Role)
                .Include(x => x.AddedBy)
                .Include(x => x.Group))
            .OrderBy(x => x.RoleId, OrderDirection.Descend)
            .ApplyOrdering()
            .SetEFFilteredEntries("User.UserName", "Role.Name", "AddedBy.UserName");
    }

    public override Table<GroupMember, int> BuildTable()
    {
        return ListTableBuilder<GroupMember, int>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }

    public override Task<bool> Authorize(QueryModel query) => rightService.HasGroupMemberListReadRight(ExtractGroupId(query));

    private static int ExtractGroupId(QueryModel query) => int.Parse(query.ExtraParams["groupId"].ToString() ?? "0");
}

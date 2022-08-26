using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table.Enums;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Logic.Services.Groups;

public class GroupActionLogTableService : TableService<GroupActionLog, long>, IGroupActionLogTableService
{
    private readonly IGroupActionLogService groupActionLogService;

    public GroupActionLogTableService(IGroupActionLogService groupActionLogService)
    {
        this.groupActionLogService = groupActionLogService;
        Initialize();
    }

    public override Configuration<GroupActionLog, long> BuildConfiguration()
    {
        return Configuration<GroupActionLog, long>
            .Build("group-log-table")
            .SetTitle("Action Logs")
            .AddColumn(Column<GroupActionLog, long>.Build("text")
                .SetTitle("Text")
                .AddValueGetter(obj => obj.Text))
            .AddColumn(Column<GroupActionLog, long>.Build("performer")
                .SetTitle("Performer")
                .AddValueGetter(obj => ObjectHelper.MapOrDefault(obj.Performer, p => p.UserName) ?? "N/A")
                .SetWidth(320))
            .AddColumn(Column<GroupActionLog, long>.Build("creation")
                .SetTitle("Creation")
                .AddValueGetter(obj => obj.Creation)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(180))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true));
    }

    public override DataSource<GroupActionLog, long> BuildDataSource()
    {
        return ListTableDataSource<GroupActionLog, long>.Build((query) => groupActionLogService.GetListAsQuery(x => x.GroupId == int.Parse(query.ExtraParams["groupId"].ToString() ?? "0")))
            .ApplyDefaultOrdering(x => x.Creation);
    }

    public override Table<GroupActionLog, long> BuildTable()
    {
        return ListTableBuilder<GroupActionLog, long>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }
}

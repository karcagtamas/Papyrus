using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table.Enums;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Logic.Services.Groups;

public class GroupActionLogTableService : TableService<ActionLog, long>, IGroupActionLogTableService
{
    private readonly IGroupActionLogService groupActionLogService;

    public GroupActionLogTableService(IGroupActionLogService groupActionLogService)
    {
        this.groupActionLogService = groupActionLogService;
        Initialize();
    }

    public override Configuration<ActionLog, long> BuildConfiguration()
    {
        return Configuration<ActionLog, long>
            .Build("group-log-table")
            .SetTitle("Action Logs", "Table.Title")
            .AddColumn(Column<ActionLog, long>.Build("text")
                .SetTitle("Text", "TableColumn.Text")
                .AddValueGetter(obj => obj.Text))
            .AddColumn(Column<ActionLog, long>.Build("performer")
                .SetTitle("Performer", "TableColumn.Performer")
                .AddValueGetter(obj => ObjectHelper.MapOrDefault(obj.Performer, p => p.UserName) ?? "N/A")
                .SetWidth(320))
            .AddColumn(Column<ActionLog, long>.Build("creation")
                .SetTitle("Creation", "TableColumn.Creation")
                .AddValueGetter(obj => obj.Creation)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(180))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true));
    }

    public override DataSource<ActionLog, long> BuildDataSource()
    {
        return ListTableDataSource<ActionLog, long>.Build((query) => groupActionLogService.GetQuery(int.Parse(query.ExtraParams["groupId"].ToString() ?? "0")))
            .OrderBy(x => x.Creation, OrderDirection.Descend)
            .ApplyOrdering();
    }

    public override Table<ActionLog, long> BuildTable()
    {
        return ListTableBuilder<ActionLog, long>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }
}

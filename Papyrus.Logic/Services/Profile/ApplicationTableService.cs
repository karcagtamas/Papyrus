using KarcagS.Common.Tools.Services;
using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table.Enums;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Logic.Services.Profile.Interfaces;

namespace Papyrus.Logic.Services.Profile;

public class ApplicationTableService : TableService<Application, string>, IApplicationTableService
{
    private readonly IApplicationService applicationService;
    private readonly IUtilsService<string> utils;

    public ApplicationTableService(IApplicationService applicationService, IUtilsService<string> utils)
    {
        this.applicationService = applicationService;
        this.utils = utils;
        Initialize();
    }

    public override Configuration<Application, string> BuildConfiguration()
    {
        return Configuration<Application, string>
            .Build("app-table")
            .SetTitle("Application", "Table.Title")
            .AddColumn(Column<Application, string>.Build("name")
                .SetTitle("Name", "TableColumn.Name")
                .AddValueGetter(obj => obj.Name))
            .AddColumn(Column<Application, string>.Build("public-id")
                .SetTitle("Public Id", "TableColumn.PublicId")
                .AddValueGetter(obj => obj.PublicId))
            .AddColumn(Column<Application, string>.Build("creation")
                .SetTitle("Creation", "TableColumn.Creation")
                .AddValueGetter(obj => obj.Creation)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(190))
            .AddColumn(Column<Application, string>.Build("last-update")
                .SetTitle("Last Update", "TableColumn.LastUpdate")
                .AddValueGetter(obj => obj.LastUpdate)
                .SetFormatter(ColumnFormatter.Date)
                .SetWidth(190));
    }

    public override DataSource<Application, string> BuildDataSource()
    {
        return ListTableDataSource<Application, string>.Build((query) =>
        {
            var userId = utils.GetRequiredCurrentUserId();
            return applicationService.GetListAsQuery(x => x.UserId == userId);
        })
            .OrderBy(x => x.Creation, OrderDirection.Descend)
            .ApplyOrdering();
    }

    public override Table<Application, string> BuildTable()
    {
        return ListTableBuilder<Application, string>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }
}

using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Logic.Services.Notes;

public class NoteActionLogTableService : TableService<ActionLog, long>, INoteActionLogTableService
{
    private readonly INoteActionLogService noteActionLogService;
    private readonly IRightService rightService;

    public NoteActionLogTableService(IAuthorizationService authorization, INoteActionLogService noteActionLogService, IRightService rightService)
    {
        this.noteActionLogService = noteActionLogService;
        this.rightService = rightService;
        Initialize();
    }

    public override Configuration<ActionLog, long> BuildConfiguration()
    {
        return Configuration<ActionLog, long>
            .Build("note-log-table")
            .SetTitle("Performed Actions", "Table.Title")
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
        return ListTableDataSource<ActionLog, long>.Build((query) => noteActionLogService.GetQuery(ExtractNoteId(query)))
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

    public override Task<bool> Authorize(QueryModel query) => rightService.HasNoteLogListReadRight(ExtractNoteId(query));

    private static string ExtractNoteId(QueryModel query) => query.ExtraParams["noteId"].ToString() ?? "";
}

using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table.Enums;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Logic.Services.Notes;

public class NoteActionLogTableService : TableService<NoteActionLog, long>, INoteActionLogTableService
{
    private readonly INoteActionLogService noteActionLogService;

    public NoteActionLogTableService(INoteActionLogService noteActionLogService)
    {
        this.noteActionLogService = noteActionLogService;
        Initialize();
    }

    public override Configuration<NoteActionLog, long> BuildConfiguration()
    {
        return Configuration<NoteActionLog, long>
           .Build("note-log-table")
           .SetTitle("Performed Actions")
           .AddColumn(Column<NoteActionLog, long>.Build("text")
               .SetTitle("Text")
               .AddValueGetter(obj => obj.Text))
           .AddColumn(Column<NoteActionLog, long>.Build("performer")
               .SetTitle("Performer")
               .AddValueGetter(obj => ObjectHelper.MapOrDefault(obj.Performer, p => p.UserName) ?? "N/A")
               .SetWidth(320))
           .AddColumn(Column<NoteActionLog, long>.Build("creation")
               .SetTitle("Text")
               .AddValueGetter(obj => obj.Creation)
               .SetFormatter(ColumnFormatter.Date)
               .SetWidth(180))
           .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true));
    }

    public override DataSource<NoteActionLog, long> BuildDataSource()
    {
        return ListTableDataSource<NoteActionLog, long>.Build((query) => noteActionLogService.GetListAsQuery(x => x.NoteId == query.ExtraParams["noteId"].ToString()))
            .OrderBy(x => x.Creation, OrderDirection.Descend)
            .ApplyOrdering();
    }

    public override Table<NoteActionLog, long> BuildTable()
    {
        return ListTableBuilder<NoteActionLog, long>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
    }
}

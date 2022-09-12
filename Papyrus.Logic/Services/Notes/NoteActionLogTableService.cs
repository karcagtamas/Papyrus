using KarcagS.Common.Tools.Services;
using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Logic.Services.Notes;

public class NoteActionLogTableService : TableService<ActionLog, long>, INoteActionLogTableService
{
    private readonly IAuthorizationService authorization;
    private readonly INoteActionLogService noteActionLogService;
    private readonly IUtilsService<string> utils;

    public NoteActionLogTableService(IAuthorizationService authorization, INoteActionLogService noteActionLogService, IUtilsService<string> utils)
    {
        this.authorization = authorization;
        this.noteActionLogService = noteActionLogService;
        this.utils = utils;
        Initialize();
    }

    public override Configuration<ActionLog, long> BuildConfiguration()
    {
        return Configuration<ActionLog, long>
           .Build("note-log-table")
           .SetTitle("Performed Actions")
           .AddColumn(Column<ActionLog, long>.Build("text")
               .SetTitle("Text")
               .AddValueGetter(obj => obj.Text))
           .AddColumn(Column<ActionLog, long>.Build("performer")
               .SetTitle("Performer")
               .AddValueGetter(obj => ObjectHelper.MapOrDefault(obj.Performer, p => p.UserName) ?? "N/A")
               .SetWidth(320))
           .AddColumn(Column<ActionLog, long>.Build("creation")
               .SetTitle("Text")
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

    public override Task<bool> Authorize(QueryModel query) => ReadCheck(ExtractNoteId(query));
    private async Task<bool> ReadCheck(string id)
    {
        var result = await authorization.AuthorizeAsync(utils.GetRequiredUserPrincipal(), id, NotePolicies.ReadNoteLogs.Requirements);

        return result.Succeeded;
    }

    private static string ExtractNoteId(QueryModel query) => query.ExtraParams["noteId"].ToString() ?? "";
}

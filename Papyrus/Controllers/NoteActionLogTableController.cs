using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteActionLogTableController : TableController<NoteActionLog, long>
{
    private readonly INoteActionLogTableService service;

    public NoteActionLogTableController(INoteActionLogTableService service)
    {
        this.service = service;
    }

    protected override ITableService<NoteActionLog, long> GetService() => service;
}

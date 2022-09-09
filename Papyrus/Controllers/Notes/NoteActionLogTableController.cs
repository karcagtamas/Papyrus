using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Controllers.Notes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteActionLogTableController : TableController<ActionLog, long>
{
    private readonly INoteActionLogTableService service;

    public NoteActionLogTableController(INoteActionLogTableService service)
    {
        this.service = service;
    }

    protected override ITableService<ActionLog, long> GetService() => service;
}

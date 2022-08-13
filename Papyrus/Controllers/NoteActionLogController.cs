using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes.ActionsLogs;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteActionLogController : ControllerBase
{
    private readonly INoteActionLogService noteActionLogService;

    public NoteActionLogController(INoteActionLogService noteActionLogService)
    {
        this.noteActionLogService = noteActionLogService;
    }

    [HttpGet("Note/{noteId}")]
    public List<NoteActionLogDTO> GetByGroup(string noteId) => noteActionLogService.GetByNote(noteId);
}

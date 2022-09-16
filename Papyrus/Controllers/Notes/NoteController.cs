using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Controllers.Notes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly INoteService noteService;
    private readonly IRightService rightService;

    public NoteController(INoteService noteService, IRightService rightService)
    {
        this.noteService = noteService;
        this.rightService = rightService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NoteDTO>> Get(string id)
    {
        if (!await rightService.HasNoteReadRight(id))
        {
            return new EmptyResult();
        }

        return noteService.GetMapped<NoteDTO>(id);
    }

    [HttpGet("Light/{id}")]
    public async Task<ActionResult<NoteLightDTO>> GetLight(string id)
    {
        if (!await rightService.HasNoteReadRight(id))
        {
            return new EmptyResult();
        }

        return noteService.GetMapped<NoteLightDTO>(id);
    }

    [HttpGet("Group/{groupId}")]
    public async Task<ActionResult<List<NoteLightDTO>>> GetByGroup(int groupId, [FromQuery] NoteFilterQueryModel query)
    {
        if (!await rightService.HasGroupNoteListReadRight(groupId))
        {
            return new EmptyResult();
        }

        return noteService.GetByGroup(groupId, query);
    }

    [HttpGet("User")]
    public List<NoteLightDTO> GetByUser([FromQuery] NoteFilterQueryModel query) => noteService.GetByUser(query);

    [HttpPost]
    public async Task<ActionResult<NoteCreationDTO>> CreateEmpty([FromBody] NoteCreateModel model)
    {
        var groupId = model.GroupId;

        if (ObjectHelper.IsNotNull(groupId) && !await rightService.HasGroupNoteCreateRight((int)groupId))
        {
            return new EmptyResult();
        }

        return noteService.CreateEmpty(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] NoteModel model)
    {
        if (!await rightService.HasNoteEditRight(id))
        {
            return new EmptyResult();
        }

        noteService.UpdateWithTags(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!await rightService.HasNoteDeleteRight(id))
        {
            return new EmptyResult();
        }

        noteService.Delete(id);

        return Ok();
    }

    [HttpGet("{id}/Rights")]
    public async Task<ActionResult<NoteRightsDTO>> GetRights(string id)
    {
        if (!await rightService.HasNoteEditRight(id))
        {
            return new EmptyResult();
        }

        return await noteService.GetRights(id);
    }
}

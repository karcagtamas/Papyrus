using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;
using Papyrus.Utils;

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
    public Task<ActionResult<NoteDTO>> Get(string id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasNoteReadRight(id),
            () => noteService.GetMapped<NoteDTO>(id));
    }

    [HttpGet("Light/{id}")]
    public Task<ActionResult<NoteLightDTO>> GetLight(string id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasNoteReadRight(id),
            () => noteService.GetMapped<NoteLightDTO>(id));
    }

    [HttpGet("Filtered")]
    public Task<ActionResult<List<NoteLightDTO>>> GetFiltered([FromQuery] NoteFilterQueryModel query, [FromQuery] int? group)
    {
        return ControllerAuthorizationHelper.Authorize(
            async () => ObjectHelper.IsNull(group) || await rightService.HasGroupNoteListReadRight((int)group),
            () => noteService.GetFiltered(query, group));
    }

    [HttpPost]
    public Task<ActionResult<NoteCreationDTO>> CreateEmpty([FromBody] NoteCreateModel model)
    {
        var groupId = model.GroupId;

        return ControllerAuthorizationHelper.Authorize(
            async () => ObjectHelper.IsNull(groupId) || await rightService.HasGroupNoteCreateRight((int)groupId),
            () => noteService.CreateEmpty(model));
    }

    [HttpPut("{id}")]
    public Task<ActionResult> Update(string id, [FromBody] NoteModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasNoteEditRight(id),
            () => noteService.UpdateWithTags(id, model));
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> Delete(string id)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasNoteDeleteRight(id),
            () => noteService.DeleteById(id));
    }

    [HttpGet("{id}/Rights")]
    public Task<NoteRightsDTO> GetRights(string id) => noteService.GetRights(id);

    [HttpGet("Search")]
    [AllowAnonymous]
    public List<SearchResultDTO> Search([FromQuery] SearchQueryModel query) => noteService.Search(query);

    [HttpGet("Exists")]
    public bool Exists([FromQuery] string parentFolder, [FromQuery] string title, [FromQuery] string? id) => noteService.Exists(parentFolder, title, id);

    [HttpPost("Access")]
    public void Access([FromBody] string id) => noteService.Access(id);

    [HttpGet("Access/Recent")]
    public List<NoteDashboardDTO> GetRecentNoteAccesses() => noteService.GetRecentNoteAccesses(rightService);

    [HttpGet("Access/Common")]
    public List<NoteDashboardDTO> GetMostCommonNoteAccesses() => noteService.GetMostCommonNoteAccesses(rightService);
}

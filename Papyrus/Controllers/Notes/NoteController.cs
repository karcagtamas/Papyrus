using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Controllers.Notes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly IAuthorizationService authorization;
    private readonly INoteService noteService;

    public NoteController(IAuthorizationService authorization, INoteService noteService)
    {
        this.authorization = authorization;
        this.noteService = noteService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NoteDTO>> Get(string id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return noteService.GetMapped<NoteDTO>(id);
    }

    [HttpGet("Light/{id}")]
    public async Task<ActionResult<NoteLightDTO>> GetLight(string id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return noteService.GetMapped<NoteLightDTO>(id);
    }

    [HttpGet("Group/{groupId}")]
    public async Task<ActionResult<List<NoteLightDTO>>> GetByGroup(int groupId, [FromQuery] NoteFilterQueryModel query)
    {
        if (!await ReadListCheck(groupId))
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

        if (ObjectHelper.IsNotNull(groupId) && !await CreateCheck((int)groupId))
        {
            return new EmptyResult();
        }

        return noteService.CreateEmpty(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] NoteModel model)
    {
        if (!await EditCheck(id))
        {
            return new EmptyResult();
        }

        noteService.UpdateWithTags(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!await DeleteCheck(id))
        {
            return new EmptyResult();
        }

        noteService.Delete(id);

        return Ok();
    }

    [HttpGet("{id}/Rights")]
    public async Task<ActionResult<NoteRightsDTO>> GetRights(string id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return await noteService.GetRights(id);
    }

    private async Task<bool> ReadCheck(string id)
    {
        var result = await authorization.AuthorizeAsync(User, id, NotePolicies.ReadNote.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> EditCheck(string id)
    {
        var result = await authorization.AuthorizeAsync(User, id, NotePolicies.EditNote.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> DeleteCheck(string id)
    {
        var result = await authorization.AuthorizeAsync(User, id, NotePolicies.DeleteNote.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> CreateCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.CreateNote.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> ReadListCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.ReadNotes.Requirements);

        return result.Succeeded;
    }
}

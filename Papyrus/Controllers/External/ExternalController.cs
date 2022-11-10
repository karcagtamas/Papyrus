using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.External.Interfaces;
using Papyrus.Shared.DTOs.External;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Controllers.External;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class ExternalController : ControllerBase
{
    private readonly IExternalService service;

    public ExternalController(IExternalService service)
    {
        this.service = service;

    }

    [HttpGet("Notes")]
    public List<NoteExtDTO> GetNotes([FromQuery] ApplicationQueryModel query) => service.GetNotes(query);

    [HttpGet("Notes/{id}")]
    public NoteContentExtDTO GetNote(string noteId, [FromQuery] ApplicationQueryModel query) => service.GetNote(query, noteId);

    [HttpGet("Tags")]
    public IActionResult GetTags([FromQuery] ApplicationQueryModel query, [FromQuery] string? type)
    {
        var inTree = type == "tree";

        return Ok(inTree ? service.GetTags<TagTreeExtDTO>(query, inTree) : service.GetTags<TagExtDTO>(query, inTree));
    }

    [HttpGet("Groups")]
    public List<GroupListExtDTO> GetGroups([FromQuery] ApplicationQueryModel query) => service.GetGroups(query);

    [HttpGet("Groups/{groupId}")]
    public GroupExtDTO GetGroup(int groupId, [FromQuery] ApplicationQueryModel query) => service.GetGroup(query, groupId);

    [HttpGet("Groups/{groupId}/Notes")]
    public List<NoteExtDTO> GetGroupNotes(int groupId, [FromQuery] ApplicationQueryModel query) => service.GetGroupNotes(query, groupId);

    [HttpGet("Groups/{groupId}/Note/{noteId}")]
    public void GetGroupNote(int groupId, string noteId, [FromQuery] ApplicationQueryModel query) { }

    [HttpGet("Groups/{groupId}/Tags")]
    public IActionResult GetGroupTags(int groupId, [FromQuery] ApplicationQueryModel query, [FromQuery] string? type)
    {
        var inTree = type == "tree";

        return Ok(inTree ? service.GetGroupTags<TagTreeExtDTO>(query, groupId, inTree) : service.GetGroupTags<TagExtDTO>(query, groupId, inTree));
    }

    [HttpGet("Groups/{groupId}/Members")]
    public void GetGroupMembers(int groupId, [FromQuery] ApplicationQueryModel query) { }
}

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
    public void GetNote(string id, [FromQuery] ApplicationQueryModel query) { }

    [HttpGet("Tags")]
    public IActionResult GetTags([FromQuery] ApplicationQueryModel query, [FromQuery] string? type)
    {
        if (type == "tree")
        {
            return Ok(service.GetTagsInTree(query));
        }

        return Ok(service.GetTagsInList(query));
    }

    [HttpGet("Groups")]
    public List<GroupExtDTO> GetGroups([FromQuery] ApplicationQueryModel query) => service.GetGroups(query);

    [HttpGet("Groups/{groupId}")]
    public void GetGroup(int groupId, [FromQuery] ApplicationQueryModel query) { }

    [HttpGet("Groups/{groupId}/Notes")]
    public void GetGroupNotes(int groupId, [FromQuery] ApplicationQueryModel query) { }

    [HttpGet("Groups/{groupId}/Tags")]
    public void GetGroupTags(int groupId, [FromQuery] ApplicationQueryModel query) { }

    [HttpGet("Groups/{groupId}/Members")]
    public void GetGroupMembers(int groupId, [FromQuery] ApplicationQueryModel query) { }
}

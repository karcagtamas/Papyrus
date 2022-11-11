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
public class TagController : ControllerBase
{
    private readonly ITagService tagService;
    private readonly IRightService rightService;

    public TagController(ITagService tagService, IRightService rightService)
    {
        this.tagService = tagService;
        this.rightService = rightService;
    }

    [HttpGet]
    public Task<ActionResult<List<NoteTagDTO>>> GetList([FromQuery] int? groupId)
    {
        return ControllerAuthorizationHelper.Authorize(
            async () => ObjectHelper.IsNull(groupId) || await rightService.HasGroupTagListReadRight((int)groupId),
            () => tagService.GetList(groupId));
    }

    [HttpGet("Tree")]
    public Task<ActionResult<List<TagTreeItemDTO>>> GetTree([FromQuery] int? groupId, [FromQuery] int? filteredTag)
    {
        return ControllerAuthorizationHelper.Authorize(
            async () => ObjectHelper.IsNull(groupId) || await rightService.HasGroupTagListReadRight((int)groupId),
            () => tagService.GetTree(groupId, filteredTag));
    }

    [HttpGet("{id}")]
    public Task<ActionResult<TagDTO>> Get(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasTagReadRight(id),
            () => tagService.GetMapped<TagDTO>(id));
    }

    [HttpGet("{id}/Path")]
    public Task<ActionResult<TagPathDTO>> GetPath(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasTagReadRight(id),
            () => tagService.GetPath(id));
    }

    [HttpPost]
    public Task<ActionResult> Create([FromBody] TagModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            async () => ObjectHelper.IsNull(model.GroupId) || await rightService.HasGroupTagCreateRight((int)model.GroupId),
            () => tagService.CreateFromModel(model));
    }

    [HttpPut("{id}")]
    public Task<ActionResult> Update(int id, [FromBody] TagModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
           () => rightService.HasTagEditRight(id),
           () => tagService.UpdateByModel(id, model));
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> Remove(int id)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
           () => rightService.HasTagEditRight(id),
           () => tagService.DeleteById(id));
    }
}

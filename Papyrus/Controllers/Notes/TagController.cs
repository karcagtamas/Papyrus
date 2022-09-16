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
    public async Task<ActionResult<List<NoteTagDTO>>> GetList([FromQuery] int? groupId)
    {
        if (ObjectHelper.IsNotNull(groupId) && !await rightService.HasGroupTagListReadRight((int)groupId))
        {
            return new EmptyResult();
        }

        return tagService.GetList(groupId);
    }

    [HttpGet("Tree")]
    public async Task<ActionResult<List<TagTreeItemDTO>>> GetTree([FromQuery] int? groupId, [FromQuery] int? filteredTag)
    {
        if (ObjectHelper.IsNotNull(groupId) && !await rightService.HasGroupTagListReadRight((int)groupId))
        {
            return new EmptyResult();
        }

        return tagService.GetTree(groupId, filteredTag);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> Get(int id)
    {
        if (!await rightService.HasTagReadRight(id))
        {
            return new EmptyResult();
        }

        return tagService.GetMapped<TagDTO>(id);
    }

    [HttpGet("{id}/Path")]
    public async Task<ActionResult<TagPathDTO>> GetPath(int id)
    {
        if (!await rightService.HasTagReadRight(id))
        {
            return new EmptyResult();
        }

        return tagService.GetPath(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TagModel model)
    {
        if (ObjectHelper.IsNotNull(model.GroupId) && !await rightService.HasGroupTagCreateRight((int)model.GroupId))
        {
            return new EmptyResult();
        }

        tagService.CreateFromModel(model);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] TagModel model)
    {
        if (!await rightService.HasTagEditRight(id))
        {
            return new EmptyResult();
        }

        tagService.UpdateByModel(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remove(int id)
    {
        if (!await rightService.HasTagEditRight(id))
        {
            return new EmptyResult();
        }

        tagService.DeleteById(id);

        return Ok();
    }
}

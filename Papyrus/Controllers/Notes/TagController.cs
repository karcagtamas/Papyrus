using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Controllers.Notes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TagController : ControllerBase
{
    private readonly IAuthorizationService authorization;
    private readonly ITagService tagService;

    public TagController(IAuthorizationService authorization, ITagService tagService)
    {
        this.authorization = authorization;
        this.tagService = tagService;
    }

    [HttpGet("Group/{groupId}")]
    public async Task<ActionResult<List<TagDTO>>> GetByGroup(int groupId)
    {
        if (!await ReadListCheck(groupId))
        {
            return new EmptyResult();
        }

        return tagService.GetByGroup(groupId);
    }

    [HttpGet("Tree")]
    public async Task<ActionResult<List<TagTreeItemDTO>>> GetTree([FromQuery] int? groupId, [FromQuery] int? filteredTag)
    {
        if (ObjectHelper.IsNotNull(groupId) && !await ReadListCheck((int)groupId))
        {
            return new EmptyResult();
        }

        return tagService.GetTree(groupId, filteredTag);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> Get(int id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return tagService.GetMapped<TagDTO>(id);
    }

    [HttpGet("{id}/Path")]
    public async Task<ActionResult<TagPathDTO>> GetPath(int id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return tagService.GetPath(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TagModel model)
    {
        if (ObjectHelper.IsNotNull(model.GroupId) && !await CreateCheck((int)model.GroupId))
        {
            return new EmptyResult();
        }

        tagService.CreateFromModel(model);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] TagModel model)
    {
        if (!await EditCheck(id))
        {
            return new EmptyResult();
        }

        tagService.UpdateByModel(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remove(int id)
    {
        if (!await EditCheck(id))
        {
            return new EmptyResult();
        }

        tagService.DeleteById(id);

        return Ok();
    }

    private async Task<bool> EditCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, TagPolicies.EditTag.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> CreateCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.CreateTag.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> ReadListCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.ReadTags.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> ReadCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, TagPolicies.ReadTag.Requirements);

        return result.Succeeded;
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Controllers.Groups;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupService groupService;
    private readonly IRightService rightService;

    public GroupController(IGroupService groupService, IRightService rightService)
    {
        this.rightService = rightService;
        this.groupService = groupService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public List<GroupListDTO> GetAll() => groupService.MapFromQuery<GroupListDTO>(groupService.GetAllAsQuery().Include(x => x.Owner)).ToList();

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDTO>> Get(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        var group = groupService.GetMapped<GroupDTO>(id);

        return group;
    }

    [HttpGet("User")]
    public List<GroupListDTO> GetUserList([FromQuery] bool hideClosed = false) => groupService.GetUserList(hideClosed);

    [HttpPost]
    public void Create([FromBody] GroupModel model) => groupService.CreateFromModel(model);

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] GroupModel model)
    {
        if (!await rightService.HasGroupEditRight(id))
        {
            return new EmptyResult();
        }

        groupService.UpdateByModel(id, model);

        return Ok();
    }

    [HttpGet("{id}/Rights/Pages")]
    public async Task<ActionResult<GroupPageRightsDTO>> GetPageRights(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        return await groupService.GetPageRights(id);
    }

    [HttpGet("{id}/Rights")]
    public async Task<ActionResult<GroupRightsDTO>> GetRights(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        return await groupService.GetRights(id);
    }

    [HttpGet("{id}/Rights/Tag")]
    public async Task<ActionResult<GroupTagRightsDTO>> GetTagRights(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        return await groupService.GetTagRights(id);
    }

    [HttpGet("{id}/Rights/Member")]
    public async Task<ActionResult<GroupMemberRightsDTO>> GetMemberRights(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        return await groupService.GetMemberRights(id);
    }

    [HttpGet("{id}/Rights/Role")]
    public async Task<ActionResult<GroupRoleRightsDTO>> GetRoleRights(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        return await groupService.GetRoleRights(id);
    }

    [HttpGet("{id}/Rights/Note")]
    public async Task<ActionResult<GroupNoteRightsDTO>> GetNoteRights(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        return await groupService.GetNoteRights(id);
    }

    [HttpPut("{id}/Close")]
    public async Task<ActionResult> Close(int id)
    {
        if (!await rightService.HasGroupCloseOpenRight(id))
        {
            return new EmptyResult();
        }

        await groupService.Close(id);

        return Ok();
    }

    [HttpPut("{id}/Open")]
    public async Task<ActionResult> Open(int id)
    {
        if (!await rightService.HasGroupCloseOpenRight(id))
        {
            return new EmptyResult();
        }

        await groupService.Open(id);

        return Ok();
    }

    [HttpPut("{id}/Remove")]
    public async Task<ActionResult> Remove(int id)
    {
        if (!await rightService.HasGroupRemoveRight(id))
        {
            return new EmptyResult();
        }

        await groupService.Remove(id);

        return Ok();
    }

    [HttpGet("{id}/RecentEdit")]
    public async Task<ActionResult<List<GroupNoteListDTO>>> GetRecentEdits(int id)
    {
        if (!await rightService.HasGroupReadRight(id))
        {
            return new EmptyResult();
        }

        return groupService.GetRecentEdits(id);
    }
}

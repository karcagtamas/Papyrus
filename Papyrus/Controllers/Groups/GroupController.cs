using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Controllers.Groups;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IAuthorizationService authorization;
    private readonly IGroupService groupService;

    public GroupController(IAuthorizationService authorization, IGroupService groupService)
    {
        this.authorization = authorization;
        this.groupService = groupService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public List<GroupListDTO> GetAll() => groupService.GetAllMapped<GroupListDTO>().ToList();

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDTO>> Get(int id)
    {
        if (!await ReadCheck(id))
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
        if (!await EditCheck(id))
        {
            return new EmptyResult();
        }

        groupService.UpdateByModel(id, model);

        return Ok();
    }

    [HttpGet("{id}/Rights")]
    public async Task<ActionResult<GroupRightsDTO>> GetRights(int id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return groupService.GetRights(id);
    }

    [HttpGet("{id}/Rights/Tag")]
    public async Task<ActionResult<GroupTagRightsDTO>> GetTagRights(int id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return groupService.GetTagRights(id);
    }

    [HttpGet("{id}/Rights/Member")]
    public async Task<ActionResult<GroupMemberRightsDTO>> GetMemberRights(int id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return groupService.GetMemberRights(id);
    }

    [HttpGet("{id}/Rights/Role")]
    public async Task<ActionResult<GroupRoleRightsDTO>> GetRoleRights(int id)
    {
        if (!await ReadCheck(id))
        {
            return new EmptyResult();
        }

        return groupService.GetRoleRights(id);
    }

    [HttpPut("{id}/Close")]
    public async Task<ActionResult> Close(int id)
    {
        if (!await CloseOpenCheck(id))
        {
            return new EmptyResult();
        }

        groupService.Close(id);

        return Ok();
    }

    [HttpPut("{id}/Open")]
    public async Task<ActionResult> Open(int id)
    {
        if (!await CloseOpenCheck(id))
        {
            return new EmptyResult();
        }

        groupService.Open(id);

        return Ok();
    }

    [HttpPut("{id}/Remove")]
    public async Task<ActionResult> Remove(int id)
    {
        if (!await RemoveCheck(id))
        {
            return new EmptyResult();
        }

        groupService.Remove(id);

        return Ok();
    }

    private async Task<bool> ReadCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.ReadGroup.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> EditCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.EditGroup.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> CloseOpenCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.CloseOpenGroup.Requirements);

        return result.Succeeded;
    }

    private async Task<bool> RemoveCheck(int id)
    {
        var result = await authorization.AuthorizeAsync(User, id, GroupPolicies.RemoveGroup.Requirements);

        return result.Succeeded;
    }
}

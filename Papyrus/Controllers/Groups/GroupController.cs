using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;
using Papyrus.Shared.Models.Groups;
using Papyrus.Utils;

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
    public Task<ActionResult<GroupDTO>> Get(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetMapped<GroupDTO>(id));
    }

    [HttpGet("User")]
    public List<GroupListDTO> GetUserList([FromQuery] bool hideClosed = false) => groupService.GetUserList(hideClosed);

    [HttpPost]
    public void Create([FromBody] GroupModel model) => groupService.CreateFromModel(model);

    [HttpPut("{id}")]
    public Task<ActionResult> Update(int id, [FromBody] GroupModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupEditRight(id),
            () => groupService.UpdateByModel(id, model));
    }

    [HttpGet("{id}/Rights/Pages")]
    public Task<ActionResult<GroupPageRightsDTO>> GetPageRights(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetPageRights(id));
    }

    [HttpGet("{id}/Rights")]
    public Task<ActionResult<GroupRightsDTO>> GetRights(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetRights(id));
    }

    [HttpGet("{id}/Rights/Tag")]
    public Task<ActionResult<GroupTagRightsDTO>> GetTagRights(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetTagRights(id));
    }

    [HttpGet("{id}/Rights/Member")]
    public Task<ActionResult<GroupMemberRightsDTO>> GetMemberRights(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetMemberRights(id));
    }

    [HttpGet("{id}/Rights/Role")]
    public Task<ActionResult<GroupRoleRightsDTO>> GetRoleRights(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetRoleRights(id));
    }

    [HttpGet("{id}/Rights/Note")]
    public Task<ActionResult<GroupNoteRightsDTO>> GetNoteRights(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetNoteRights(id));
    }

    [HttpPut("{id}/Close")]
    public Task<ActionResult> Close(int id)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupCloseRight(id),
            async () => await groupService.Close(id));
    }

    [HttpPut("{id}/Open")]
    public Task<ActionResult> Open(int id)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupOpenRight(id),
            async () => await groupService.Open(id));
    }

    [HttpPut("{id}/Remove")]
    public Task<ActionResult> Remove(int id)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupRemoveRight(id),
            async () => await groupService.Remove(id));
    }

    [HttpGet("{id}/RecentEdit")]
    public Task<ActionResult<List<GroupNoteListDTO>>> GetRecentEdits(int id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupReadRight(id),
            () => groupService.GetRecentEdits(id));
    }
}

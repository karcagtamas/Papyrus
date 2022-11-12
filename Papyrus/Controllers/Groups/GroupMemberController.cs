using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;
using Papyrus.Utils;

namespace Papyrus.Controllers.Groups;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupMemberController : ControllerBase
{
    private readonly IGroupMemberService groupMemberService;
    private readonly IRightService rightService;

    public GroupMemberController(IGroupMemberService groupMemberService, IRightService rightService)
    {
        this.rightService = rightService;
        this.groupMemberService = groupMemberService;
    }

    [HttpGet("{id}")]
    public Task<ActionResult<GroupMemberDTO>> Get(int id)
    {
        var member = groupMemberService.GetMapped<GroupMemberDTO>(id);

        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupMemberListReadRight(member.GroupId),
            () => member);
    }

    [HttpPost]
    public Task<ActionResult> AddMember([FromBody] GroupMemberCreateModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupMemberListEditRight(model.GroupId),
            () => groupMemberService.CreateFromModelWithDefaultRole(model));
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> RemoveMember(int id)
    {
        var member = groupMemberService.Get(id);

        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupMemberListEditRight(member.GroupId),
            () => groupMemberService.Delete(member));
    }

    [HttpPut("{id}")]
    public Task<ActionResult> EditMember(int id, [FromBody] GroupMemberUpdateModel model)
    {
        var member = groupMemberService.Get(id);

        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupMemberListEditRight(member.GroupId),
            () => groupMemberService.UpdateByModel(id, model));
    }

    [HttpGet("UserKeys/{groupId}")]
    public Task<ActionResult<List<string>>> GetMemberKeys(int groupId, [FromQuery] List<int> memberIds)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupMemberListReadRight(groupId),
            () => groupMemberService.GetMemberKeys(groupId, memberIds));
    }
}

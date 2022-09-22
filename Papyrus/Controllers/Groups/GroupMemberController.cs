using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

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
    public async Task<ActionResult<GroupMemberDTO>> Get(int id)
    {
        var member = groupMemberService.GetMapped<GroupMemberDTO>(id);

        if (!await rightService.HasGroupMemberListReadRight(member.GroupId))
        {
            return new EmptyResult();
        }

        return member;
    }

    [HttpPost]
    public async Task<ActionResult> AddMember([FromBody] GroupMemberCreateModel model)
    {
        if (!await rightService.HasGroupMemberListEditRight(model.GroupId))
        {
            return new EmptyResult();
        }

        groupMemberService.CreateFromModelWithDefaultRole(model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveMember(int id)
    {
        var member = groupMemberService.Get(id);

        if (!await rightService.HasGroupMemberListEditRight(member.GroupId))
        {
            return new EmptyResult();
        }

        groupMemberService.Delete(member);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditMember(int id, [FromBody] GroupMemberUpdateModel model)
    {
        var member = groupMemberService.Get(id);

        if (!await rightService.HasGroupMemberListEditRight(member.GroupId))
        {
            return new EmptyResult();
        }

        groupMemberService.UpdateByModel(id, model);

        return Ok();
    }

    [HttpGet("UserKeys/{groupId}")]
    public async Task<ActionResult<List<string>>> GetMemberKeys(int groupId, [FromQuery] List<int> memberIds)
    {
        if (!await rightService.HasGroupMemberListReadRight(groupId))
        {
            return new EmptyResult();
        }

        return groupMemberService.GetMemberKeys(groupId, memberIds);
    }
}

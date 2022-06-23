using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupMemberController : ControllerBase
{
    private readonly IGroupMemberService groupMemberService;

    public GroupMemberController(IGroupMemberService groupMemberService)
    {
        this.groupMemberService = groupMemberService;
    }

    [HttpGet("Group/{groupId}")]
    public List<GroupMemberDTO> GetByGroup(int groupId)
    {
        return groupMemberService.GetMappedList<GroupMemberDTO>(x => x.GroupId == groupId)
            .OrderBy(x => x.Role.Id)
            .ThenBy(x => x.User.UserName)
            .ToList();
    }

    [HttpPost]
    public void AddMember([FromBody] GroupMemberCreateModel model)
    {
        groupMemberService.CreateFromModelWithDefaultRole(model);
    }

    [HttpDelete("{id}")]
    public void RemoveMember(int id)
    {
        groupMemberService.DeleteById(id);
    }

    [HttpPut("{id}")]
    public void EditMember(int id, [FromBody] GroupMemberUpdateModel model)
    {
        groupMemberService.UpdateByModel(id, model);
    }
}

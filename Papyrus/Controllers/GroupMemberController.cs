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
    public List<GroupMemberDTO> GetByGroup(int groupId, [FromQuery] string? textFilter)
    {
        return groupMemberService.GetMappedList<GroupMemberDTO>(x => x.GroupId == groupId
                && (textFilter == null
                    || x.User.UserName.Contains(textFilter)
                    || x.Role.Name.Contains(textFilter)
                    || (x.AddedBy != null && x.AddedBy.UserName.Contains(textFilter))))
            .OrderBy(x => x.Role.Id)
            .ThenBy(x => x.User.UserName)
            .ToList();
    }

    [HttpGet("{id}")]
    public GroupMemberDTO Get(int id) => groupMemberService.GetMapped<GroupMemberDTO>(id);

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

    [HttpGet("UserKeys")]
    public List<string> GetMemberKeys([FromQuery] List<int> memberIds) => groupMemberService.GetMemberKeys(memberIds);
}

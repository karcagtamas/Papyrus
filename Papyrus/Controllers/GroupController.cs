using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupService groupService;

    public GroupController(IGroupService groupService)
    {
        this.groupService = groupService;
    }

    [HttpGet]
    public List<GroupListDTO> GetAll()
    {
        return groupService.GetAllMapped<GroupListDTO>().ToList();
    }

    [HttpGet("{id}")]
    public GroupDTO Get(int id)
    {
        return groupService.GetMapped<GroupDTO>(id);
    }

    [HttpGet("User")]
    public List<GroupListDTO> GetUserList()
    {
        return groupService.GetUserList();
    }

    [HttpGet("{id}/Member")]
    public List<GroupMemberDTO> GetMembers(int id)
    {
        return groupService.GetMembers(id);
    }

    [HttpPost]
    public void Create([FromBody] GroupModel model)
    {
        groupService.CreateFromModel(model);
    }

    [HttpPut("{id}")]
    public void Update(int id, [FromBody] GroupModel model)
    {
        groupService.UpdateByModel(id, model);
    }

    [HttpPost("{id}/Member")]
    public void AddMember(int id, [FromBody] string memberId) 
    {
        groupService.AddMember(id, memberId);
    }

    [HttpDelete("{id}/Member/{groupMemberId}")]
    public void RemoveMember(int id, int groupMemberId) 
    {
        groupService.RemoveMember(id, groupMemberId);
    }
}

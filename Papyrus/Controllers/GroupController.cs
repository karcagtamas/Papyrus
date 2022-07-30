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
    public GroupDTO Get(int id) => groupService.GetMapped<GroupDTO>(id);

    [HttpGet("User")]
    public List<GroupListDTO> GetUserList([FromQuery] bool hideClosed = false) => groupService.GetUserList(hideClosed);

    [HttpPost]
    public void Create([FromBody] GroupModel model) => groupService.CreateFromModel(model);

    [HttpPut("{id}")]
    public void Update(int id, [FromBody] GroupModel model) => groupService.UpdateByModel(id, model);

    [HttpGet("{id}/Rights")]
    public GroupRightsDTO GetRights(int id) => groupService.GetRights(id);

    [HttpGet("{id}/Rights/Tag")]
    public GroupTagRightsDTO GetTagRights(int id) => groupService.GetTagRights(id);

    [HttpGet("{id}/Rights/Member")]
    public GroupMemberRightsDTO GetMemberRights(int id) => groupService.GetMemberRights(id);

    [HttpGet("{id}/Rights/Role")]
    public GroupRoleRightsDTO GetRoleRights(int id) => groupService.GetRoleRights(id);

    [HttpPut("{id}/Close")]
    public void Close(int id) => groupService.Close(id);

    [HttpPut("{id}/Open")]
    public void Open(int id) => groupService.Open(id);

    [HttpPut("{id}/Remove")]
    public void Remove(int id) => groupService.Remove(id);
}

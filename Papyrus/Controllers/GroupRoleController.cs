using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupRoleController : ControllerBase
{
    private readonly IGroupRoleService groupRoleService;

    public GroupRoleController(IGroupRoleService groupRoleService)
    {
        this.groupRoleService = groupRoleService;
    }

    [HttpGet("{id}")]
    public GroupRoleDTO Get(int id) => groupRoleService.GetMapped<GroupRoleDTO>(id);

    [HttpGet("Group/{groupId}")]
    public List<GroupRoleDTO> GetByGroup(int groupId, string? textFilter = null) => groupRoleService.GetByGroup(groupId, textFilter);

    [HttpGet("Group/{groupId}/Light")]
    public List<GroupRoleLightDTO> GetLightByGroup(int groupId) => groupRoleService.GetLightByGroup(groupId);

    [HttpPost]
    public void Create([FromBody] GroupRoleModel model) => groupRoleService.CreateFromModel(model);

    [HttpPut("{id}")]
    public void Update(int id, [FromBody] GroupRoleModel model) => groupRoleService.UpdateByModel(id, model);

    [HttpDelete("{id}")]
    public void Delete(int id) => groupRoleService.DeleteById(id);

    [HttpGet("Exists")]
    public bool Exists([FromQuery] int groupId, [FromQuery] string name) => groupRoleService.Exists(groupId, name);
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Controllers.Groups;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupRoleController : ControllerBase
{
    private readonly IGroupRoleService groupRoleService;
    private readonly IRightService rightService;

    public GroupRoleController(IGroupRoleService groupRoleService, IRightService rightService)
    {
        this.groupRoleService = groupRoleService;
        this.rightService = rightService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupRoleDTO>> Get(int id)
    {
        var role = groupRoleService.GetMapped<GroupRoleDTO>(id);

        if (!await rightService.HasGroupRoleListReadRight(role.GroupId))
        {
            return new EmptyResult();
        }

        return role;
    }

    [HttpGet("Group/{groupId}/Translated")]
    public async Task<ActionResult<List<GroupRoleDTO>>> GetTranslatedByGroup(int groupId, [FromQuery] string? textFilter = null, [FromQuery] string? lang = null)
    {
        if (!await rightService.HasGroupRoleListReadRight(groupId))
        {
            return new EmptyResult();
        }

        return groupRoleService.GetTranslatedByGroup(groupId, textFilter, lang);
    }

    [HttpGet("Group/{groupId}/Light/Translated")]
    public async Task<ActionResult<List<GroupRoleLightDTO>>> GetLightTranslatedByGroup(int groupId, [FromQuery] string? lang = null)
    {
        if (!await rightService.HasGroupRoleListReadRight(groupId))
        {
            return new EmptyResult();
        }

        return groupRoleService.GetLightTranslatedByGroup(groupId, lang);
    }

    [HttpGet("{id}/Light/Translated")]
    public async Task<ActionResult<GroupRoleLightDTO>> GetLightTranslated(int id, [FromQuery] string? lang = null)
    {
        var role = groupRoleService.GetLightTranslated(id, lang);

        if (!await rightService.HasGroupRoleListReadRight(role.GroupId))
        {
            return new EmptyResult();
        }

        return role;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] GroupRoleModel model)
    {
        if (!await rightService.HasGroupRoleListEditRight(model.GroupId))
        {
            return new EmptyResult();
        }

        groupRoleService.CreateFromModel(model);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] GroupRoleModel model)
    {
        if (!await rightService.HasGroupRoleListEditRight(model.GroupId))
        {
            return new EmptyResult();
        }

        groupRoleService.UpdateByModel(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var role = groupRoleService.Get(id);

        if (!await rightService.HasGroupRoleListEditRight(role.GroupId))
        {
            return new EmptyResult();
        }

        groupRoleService.Delete(role);

        return Ok();
    }

    [HttpGet("Exists")]
    public bool Exists([FromQuery] int groupId, [FromQuery] string name) => groupRoleService.Exists(groupId, name);
}

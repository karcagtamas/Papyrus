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
    public Task<ActionResult<GroupRoleDTO>> Get(int id)
    {
        var role = groupRoleService.GetMapped<GroupRoleDTO>(id);

        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupRoleListReadRight(role.GroupId),
            () => role);
    }

    [HttpGet("Group/{groupId}/Translated")]
    public Task<ActionResult<List<GroupRoleDTO>>> GetTranslatedByGroup(int groupId, [FromQuery] string? textFilter = null, [FromQuery] string? lang = null)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupRoleListReadRight(groupId),
            () => groupRoleService.GetTranslatedByGroup(groupId, textFilter, lang));
    }

    [HttpGet("Group/{groupId}/Light/Translated")]
    public Task<ActionResult<List<GroupRoleLightDTO>>> GetLightTranslatedByGroup(int groupId, [FromQuery] string? lang = null)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupRoleListReadRight(groupId),
            () => groupRoleService.GetLightTranslatedByGroup(groupId, lang));
    }

    [HttpGet("{id}/Light/Translated")]
    public Task<ActionResult<GroupRoleLightDTO>> GetLightTranslated(int id, [FromQuery] string? lang = null)
    {
        var role = groupRoleService.GetLightTranslated(id, lang);

        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasGroupRoleListReadRight(role.GroupId),
            () => role);
    }

    [HttpPost]
    public Task<ActionResult> Create([FromBody] GroupRoleModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupRoleListEditRight(model.GroupId),
            () => groupRoleService.CreateFromModel(model));
    }

    [HttpPut("{id}")]
    public Task<ActionResult> Update(int id, [FromBody] GroupRoleModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupRoleListEditRight(model.GroupId),
            () => groupRoleService.UpdateByModel(id, model));
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> Delete(int id)
    {
        var role = groupRoleService.Get(id);

        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasGroupRoleListEditRight(role.GroupId),
            () => groupRoleService.Delete(role));
    }

    [HttpGet("Exists")]
    public bool Exists([FromQuery] int groupId, [FromQuery] string name) => groupRoleService.Exists(groupId, name);
}

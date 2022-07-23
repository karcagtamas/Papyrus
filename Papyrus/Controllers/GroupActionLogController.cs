using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupActionLogController : ControllerBase
{
    private readonly IGroupActionLogService groupActionLogService;

    public GroupActionLogController(IGroupActionLogService groupActionLogService)
    {
        this.groupActionLogService = groupActionLogService;
    }

    [HttpGet("Group/{groupId}")]
    public List<GroupActionLogDTO> GetByGroup(int groupId) => groupActionLogService.GetByGroup(groupId);
}

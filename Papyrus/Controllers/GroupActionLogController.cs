using KarcagS.Shared.Table;
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
    public TableResult<GroupActionLogDTO> GetByGroup(int groupId, [FromQuery] int? page = null, [FromQuery] int? size = null) => groupActionLogService.GetByGroup(groupId, page, size);
}

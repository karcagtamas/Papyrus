using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Controllers.Groups;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupActionLogTableController : TableController<ActionLog, long>
{
    private readonly IGroupActionLogTableService service;

    public GroupActionLogTableController(IGroupActionLogTableService service)
    {
        this.service = service;
    }

    protected override ITableService<ActionLog, long> GetService() => service;
}

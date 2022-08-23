using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupActionLogTableController : TableController<GroupActionLog, long>
{
    private readonly IGroupActionLogTableService service;

    public GroupActionLogTableController(IGroupActionLogTableService service)
    {
        this.service = service;
    }

    public override ITableService<GroupActionLog, long> GetService() => service;
}

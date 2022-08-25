using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupRoleTableController : TableController<GroupRole, int>
{
    private readonly IGroupRoleTableService service;

    public GroupRoleTableController(IGroupRoleTableService service)
    {
        this.service = service;
    }

    protected override ITableService<GroupRole, int> GetService() => service;
}

using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Controllers.Groups;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupMemberTableController : TableController<GroupMember, int>
{
    private readonly IGroupMemberTableService service;

    public GroupMemberTableController(IGroupMemberTableService service)
    {
        this.service = service;
    }

    protected override ITableService<GroupMember, int> GetService() => service;
}

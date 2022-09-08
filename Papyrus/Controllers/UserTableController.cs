using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator,Moderator")]
public class UserTableController : TableController<User, string>
{
    private readonly IUserTableService service;

    public UserTableController(IUserTableService service)
    {
        this.service = service;
    }

    protected override ITableService<User, string> GetService() => service;
}

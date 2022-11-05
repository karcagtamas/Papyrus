using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationTableController : TableController<Application, string>
{
    private readonly IApplicationTableService service;

    public ApplicationTableController(IApplicationTableService service)
    {
        this.service = service;
    }

    protected override ITableService<Application, string> GetService() => service;
}

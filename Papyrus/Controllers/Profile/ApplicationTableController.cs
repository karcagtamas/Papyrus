using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Logic.Services.Profile.Interfaces;

namespace Papyrus.Controllers.Profile;

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

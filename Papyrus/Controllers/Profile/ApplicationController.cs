using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Controllers.Profile;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService applicationService;
    private readonly IRightService rightService;

    public ApplicationController(IApplicationService applicationService, IRightService rightService)
    {
        this.applicationService = applicationService;
        this.rightService = rightService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationDTO>> Get(string id)
    {
        if (!await rightService.HasApplicationAccessRight(id))
        {
            return new EmptyResult();
        }

        return applicationService.GetMapped<ApplicationDTO>(id);
    }

    [HttpPost]
    public void Create([FromBody] ApplicationModel model) => applicationService.CreateWithKeys(model);

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] ApplicationModel model)
    {
        if (!await rightService.HasApplicationAccessRight(id))
        {
            return new EmptyResult();
        }

        applicationService.UpdateByModel(id, model);

        return Ok();
    }

    [HttpPut("{id}/RefreshSecret")]
    public async Task<IActionResult> RefreshSecret(string id)
    {
        if (!await rightService.HasApplicationAccessRight(id))
        {
            return new EmptyResult();
        }

        applicationService.RefreshSecret(id);

        return Ok();
    }
}

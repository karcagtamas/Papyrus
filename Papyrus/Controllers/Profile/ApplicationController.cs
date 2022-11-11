using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Shared.DTOs.Profile;
using Papyrus.Shared.Models.Profile;
using Papyrus.Utils;

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
    public Task<ActionResult<ApplicationDTO>> Get(string id)
    {
        return ControllerAuthorizationHelper.Authorize(
           () => rightService.HasApplicationAccessRight(id),
           () => applicationService.GetMapped<ApplicationDTO>(id));
    }

    [HttpPost]
    public void Create([FromBody] ApplicationModel model) => applicationService.CreateWithKeys(model);

    [HttpPut("{id}")]
    public Task<ActionResult> Update(string id, [FromBody] ApplicationModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
           () => rightService.HasApplicationAccessRight(id),
           () => applicationService.UpdateByModel(id, model));
    }

    [HttpPut("{id}/RefreshSecret")]
    public Task<ActionResult> RefreshSecret(string id)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
           () => rightService.HasApplicationAccessRight(id),
           () => applicationService.RefreshSecret(id));
    }
}

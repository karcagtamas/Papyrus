using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Controllers.Profile;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationController
{
    private readonly IApplicationService applicationService;

    public ApplicationController(IApplicationService applicationService)
    {
        this.applicationService = applicationService;
    }

    [HttpGet("{id}")]
    public ApplicationDTO Get(string id) => applicationService.GetMapped<ApplicationDTO>(id);

    [HttpPost]
    public void Create([FromBody] ApplicationModel model) => applicationService.CreateWithKeys(model);

    [HttpPut("{id}")]
    public void Update(string id, [FromBody] ApplicationModel model) => applicationService.UpdateByModel(id, model);

    [HttpPut("{id}/RefreshSecret")]
    public void RefreshSecret(string id) => applicationService.RefreshSecret(id);
}

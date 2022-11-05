using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Controllers;

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
    public void Create([FromBody] ApplicationModel model) => applicationService.CreateFromModel(model);

    [HttpPut("{id}")]
    public void Update(string id, [FromBody] ApplicationModel model) => applicationService.UpdateByModel(id, model);
}

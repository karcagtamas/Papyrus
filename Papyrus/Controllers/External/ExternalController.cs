using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.External.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Controllers.External;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class ExternalController : ControllerBase
{
    private readonly IExternalService service;

    public ExternalController(IExternalService service)
    {
        this.service = service;

    }

    [HttpGet("Notes")]
    public List<NoteLightDTO> GetNotes([FromQuery] ApplicationQueryModel query) => service.GetNotes(query);
}

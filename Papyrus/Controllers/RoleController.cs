using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;

    public RoleController(IRoleService roleService)
    {
        this.roleService = roleService;
    }

    [HttpGet]
    public List<RoleDTO> GetAll() => roleService.GetAllMapped<RoleDTO>().ToList();

    [HttpGet("Translated")]
    public List<RoleDTO> GetAllTranslated([FromQuery] string? lang) => roleService.GetAllTranslated(lang);
}

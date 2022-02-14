using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteWeb.Logic.Services.Interfaces;
using NoteWeb.Shared.DTOs;
using NoteWeb.Shared.Models;

namespace NoteWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly ITokenService tokenService;

    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        this.authService = authService;
        this.tokenService = tokenService;
    }

    [HttpPost("Login")]
    public async Task<TokenDTO> Login([FromBody] LoginModel model)
    {
        return await authService.Login(model);
    }

    [HttpPost("Register")]
    public async Task Register([FromBody] RegistrationModel model)
    {
        await authService.Register(model);
    }

    [AllowAnonymous]
    [HttpGet("Refresh")]
    public async Task<TokenDTO> Refresh([FromQuery] string refreshToken, [FromQuery] string clientId)
    {
        return await tokenService.Refresh(refreshToken, clientId);
    }
}

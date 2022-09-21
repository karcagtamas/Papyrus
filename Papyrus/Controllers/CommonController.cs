using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommonController : ControllerBase
{
    private readonly ICommonService commonService;

    public CommonController(ICommonService commonService)
    {
        this.commonService = commonService;
    }

    [HttpGet("Theme/Translated")]
    [AllowAnonymous]
    public List<ThemeDTO> GetTranslatedThemeList([FromQuery] string? lang) => commonService.GetTranslatedThemeList(lang);

    [HttpGet("Theme/User")]
    public int GetUserTheme() => commonService.GetUserTheme();

    [HttpPut("Theme/User")]
    public void SetTheme([FromBody] int key) => commonService.SetUserTheme(key);
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LanguageController : ControllerBase
{
    private readonly ILanguageService languageService;

    public LanguageController(ILanguageService languageService)
    {
        this.languageService = languageService;
    }

    [HttpGet]
    public List<LanguageDTO> GetAll() => languageService.GetAllMapped<LanguageDTO>().ToList();

    [HttpGet("User")]
    public LanguageDTO? GetUserLanguage() => languageService.GetUserLanguage();

    [HttpPut("User")]
    [Authorize]
    public void SetUserLanguage([FromBody] int id) => languageService.SetUserLanguage(id);
}

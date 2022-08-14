using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Common.Interfaces;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IFileService fileService;

    public FileController(IFileService fileService)
    {
        this.fileService = fileService;
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var file = fileService.Get(id);

        if (ObjectHelper.IsNull(file))
        {
            return new EmptyResult();
        }

        return File(file.Content, file.MimeType);
    }
}

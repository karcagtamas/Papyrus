using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Controllers.Notes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FolderController : ControllerBase
{
    private readonly IFolderService folderService;

    public FolderController(IFolderService folderService)
    {
        this.folderService = folderService;
    }

    [HttpGet]
    public FolderContentDTO GetFolderContent([FromQuery] string? folder, [FromQuery] int? groupId)
    {
        // TODO: Authorize checking
        return folderService.GetContent(folder, groupId);
    }

    [HttpGet("{id}")]
    public FolderDTO Get(string id) => folderService.GetMapped<FolderDTO>(id); // TODO: Authorize

    [HttpPost]
    public void Create([FromBody] FolderModel model) => folderService.CreateFolder(model); // TODO: Authorize

    [HttpPut("{id}")]
    public void Update(string id, [FromBody] FolderModel model) => folderService.EditFolder(id, model); // TODO: Authorize
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
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
    private readonly IRightService rightService;
    private readonly INoteService noteService;

    public FolderController(IFolderService folderService, IRightService rightService, INoteService noteService)
    {
        this.folderService = folderService;
        this.rightService = rightService;
        this.noteService = noteService;
    }

    [HttpGet]
    public async Task<ActionResult<FolderContentDTO>> GetFolderContent([FromQuery] string? folder, [FromQuery] int? group)
    {
        if (ObjectHelper.IsNotNull(group) && !await rightService.HasGroupNoteListReadRight((int)group))
        {
            return new EmptyResult();
        }

        if (ObjectHelper.IsNotNull(folder) && !await rightService.HasFolderReadRight(folder))
        {
            return new EmptyResult();
        }

        return folderService.GetContent(folder, group);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FolderDTO>> Get(string id)
    {
        if (!await rightService.HasFolderReadRight(id))
        {
            return new EmptyResult();
        }

        return folderService.GetMapped<FolderDTO>(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] FolderModel model)
    {
        if (ObjectHelper.IsNotNull(model.GroupId) && !await rightService.HasGroupFolderCreateRight((int)model.GroupId))
        {
            return new EmptyResult();
        }

        folderService.CreateFolder(model);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] FolderModel model)
    {
        if (!await rightService.HasFolderManageRight(id))
        {
            return new EmptyResult();
        }

        folderService.EditFolder(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!await rightService.HasFolderManageRight(id))
        {
            return new EmptyResult();
        }

        noteService.DeleteFolder(id);

        return Ok();
    }

    [HttpGet("Exists")]
    public bool Exists([FromQuery] string parentFolder, [FromQuery] string name) => folderService.Exists(parentFolder, name);
}

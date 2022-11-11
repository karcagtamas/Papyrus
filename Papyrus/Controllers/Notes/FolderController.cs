using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;
using Papyrus.Utils;

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
    public Task<ActionResult<FolderContentDTO>> GetFolderContent([FromQuery] string? folder, [FromQuery] int? group)
    {
        return ControllerAuthorizationHelper.Authorize(
            async () => (ObjectHelper.IsNull(group) || await rightService.HasGroupNoteListReadRight((int)group))
                && (ObjectHelper.IsNull(folder) || await rightService.HasFolderReadRight(folder)),
            () => folderService.GetContent(folder, group));
    }

    [HttpGet("{id}")]
    public Task<ActionResult<FolderDTO>> Get(string id)
    {
        return ControllerAuthorizationHelper.Authorize(
            () => rightService.HasFolderReadRight(id),
            () => folderService.GetMapped<FolderDTO>(id));
    }

    [HttpPost]
    public Task<ActionResult> Create([FromBody] FolderModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            async () => ObjectHelper.IsNull(model.GroupId) || await rightService.HasGroupFolderCreateRight((int)model.GroupId),
            () => folderService.CreateFolder(model));
    }

    [HttpPut("{id}")]
    public Task<ActionResult> Update(string id, [FromBody] FolderModel model)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasFolderManageRight(id),
            () => folderService.EditFolder(id, model));
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> Delete(string id)
    {
        return ControllerAuthorizationHelper.AuthorizeVoid(
            () => rightService.HasFolderManageRight(id),
            () => noteService.DeleteFolder(id));
    }

    [HttpGet("Exists")]
    public bool Exists([FromQuery] string parentFolder, [FromQuery] string name) => folderService.Exists(parentFolder, name);
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Editor.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EditorController : ControllerBase
{
    private readonly IEditorService editorService;

    public EditorController(IEditorService editorService)
    {
        this.editorService = editorService;
    }

    [HttpGet("{id}/Members")]
    public List<UserLightDTO> GetMembers(string id) => editorService.GetMembers(id);
}

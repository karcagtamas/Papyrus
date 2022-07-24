using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly INoteService noteService;

    public NoteController(INoteService noteService)
    {
        this.noteService = noteService;
    }

    [HttpGet("{id}")]
    public NoteDTO Get(string id) => noteService.GetMapped<NoteDTO>(id);

    [HttpGet("Light/{id}")]
    public NoteLightDTO GetLight(string id) => noteService.GetMapped<NoteLightDTO>(id);

    [HttpGet("Group/{groupId}")]
    public List<NoteLightDTO> GetByGroup(int groupId) => noteService.GetByGroup(groupId);

    [HttpGet("User")]
    public List<NoteLightDTO> GetByUser() => noteService.GetByUser();

    [HttpPost]
    public NoteCreationDTO CreateEmpty([FromBody] NoteCreateModel model) => noteService.CreateEmpty(model.GroupId);

    [HttpPut("{id}")]
    public void Update(string id, [FromBody] NoteModel model) => noteService.UpdateByModel(id, model);

    [HttpDelete("{id}")]
    public void Delete(string id) => noteService.DeleteById(id);
}

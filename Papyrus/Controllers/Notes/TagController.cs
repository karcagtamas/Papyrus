using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Controllers.Notes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TagController : ControllerBase
{
    private readonly ITagService tagService;

    public TagController(ITagService tagService)
    {
        this.tagService = tagService;
    }

    [HttpGet("Group/{groupId}")]
    public List<TagDTO> GetByGroup(int groupId) => tagService.GetByGroup(groupId);

    [HttpGet("Tree")]
    public List<TagTreeItemDTO> GetTree([FromQuery] int? groupId, [FromQuery] int? filteredTag) => tagService.GetTree(groupId, filteredTag);

    [HttpGet("{id}")]
    public TagDTO Get(int id) => tagService.GetMapped<TagDTO>(id);

    [HttpGet("{id}/Path")]
    public TagPathDTO GetPath(int id) => tagService.GetPath(id);

    [HttpPost]
    public void Create([FromBody] TagModel model) => tagService.CreateFromModel(model);

    [HttpPut("{id}")]
    public void Update(int id, [FromBody] TagModel model) => tagService.UpdateByModel(id, model);

    [HttpDelete("{id}")]
    public void Remove(int id) => tagService.DeleteById(id);
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Controllers;

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

    [HttpGet("Group/{groupId}/Tree")]
    public List<GroupTagTreeItemDTO> GetTreeByGroup(int groupId) => tagService.GetTreeByGroup(groupId);

    [HttpGet("{id}")]
    public TagDTO Get(int id) => tagService.GetMapped<TagDTO>(id);

    [HttpGet("{id}/Path")]
    public TagPathDTO GetPath(int id) => tagService.GetPath(id);

    [HttpPost("Group")]
    public void CreateGroupTag([FromBody] GroupTagModel model) => tagService.CreateFromModel(model);

    [HttpPut("Group/{id}")]
    public void UpdateGroupTag(int id, [FromBody] GroupTagModel model) => tagService.UpdateByModel(id, model);

    [HttpDelete("{id}")]
    public void Remove(int id) => tagService.DeleteById(id);
}

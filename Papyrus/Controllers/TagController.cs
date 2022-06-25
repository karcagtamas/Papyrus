using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models;

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
    public List<GroupTagDTO> GetByGroup(int groupId)
    {
        return tagService.GetByGroup(groupId);
    }

    [HttpGet("Group/{groupId}/Tree")]
    public List<GroupTagTreeItemDTO> GetTreeByGroup(int groupId)
    {
        return tagService.GetTreeByGroup(groupId);
    }
}

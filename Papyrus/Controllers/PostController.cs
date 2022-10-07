using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PostController : ControllerBase
{
    private readonly IPostService postService;

    public PostController(IPostService postService)
    {
        this.postService = postService;
    }

    [HttpGet]
    public List<PostDTO> GetList()
    {
        return postService.MapFromQuery<PostDTO>(postService.GetAllAsQuery()
                .Include(x => x.Creator)
                .Include(x => x.LastUpdater)
                .OrderByDescending(x => x.Creation)
                .Take(5))
            .ToList();
    }

    [HttpGet("{id}")]
    public PostDTO Get(int id) => postService.GetMapped<PostDTO>(id);

    [HttpPost]
    [Authorize("Administrator, Moderator")]
    public void Create([FromBody] PostModel model) => postService.CreateFromModel(model);

    [HttpPut("{id}")]
    [Authorize("Administrator, Moderator")]
    public void Update(int id, [FromBody] PostModel model) => postService.UpdateByModel(id, model);

    [HttpDelete("{id}")]
    [Authorize("Administrator, Moderator")]
    public void Delete(int id) => postService.DeleteById(id);
}

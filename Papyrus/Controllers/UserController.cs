using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public List<UserListDTO> GetAll()
    {
        return userService.GetAllMapped<UserListDTO>().ToList();
    }

    [HttpGet("{id}")]
    public UserDTO Get(string id)
    {
        return userService.GetMapped<UserDTO>(id);
    }

    [HttpGet("{id}/Light")]
    public UserLightDTO GetLight(string id)
    {
        return userService.GetMapped<UserLightDTO>(id);
    }

    [HttpPut("{id}")]
    public void Update(string id, [FromBody] UserModel model)
    {
        userService.Update(id, model);
    }

    [HttpGet("Current")]
    public UserDTO Current()
    {
        return userService.GetCurrent<UserDTO>();
    }

    [HttpGet("Exists")]
    public bool Exists([FromQuery] string userName, [FromQuery] string email)
    {
        return userService.IsExist(userName, email, true);
    }

    [HttpPost("Disable")]
    public void DisableUsers([FromBody] UserDisableStatusModel statusModel)
    {
        userService.SetDisableStatus(statusModel);
    }

    [HttpPut("Image")]
    public void UpdateImage([FromBody] byte[] image)
    {
        userService.UpdateImage(image);
    }

    [HttpPut("Password")]
    public async Task UpdatePassword([FromBody] UserPasswordModel model)
    {
        await userService.UpdatePassword(model);
    }

    [HttpGet("Search")]
    public List<UserLightDTO> Search([FromQuery] string searchTerm, [FromQuery] bool ignoreCurrent, [FromQuery] List<string> ignored)
    {
        return userService.Search(searchTerm, ignoreCurrent, ignored);
    }
}

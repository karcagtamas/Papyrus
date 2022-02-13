using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteWeb.Logic.Services.Interfaces;
using NoteWeb.Shared.DTOs;
using NoteWeb.Shared.Models;

namespace NoteWeb.Controllers;

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

    [HttpPut("{id}")]
    public void Update(string id, [FromBody] UserModel model)
    {
        userService.UpdateByModel(id, model);
    }

    [HttpGet("Current")]
    public UserDTO Current()
    {
        return userService.GetCurrent<UserDTO>();
    }

    [HttpGet("Exists")]
    public bool Exists([FromQuery] string userName, [FromQuery] string email)
    {
        return userService.IsExist(userName, email);
    }
}

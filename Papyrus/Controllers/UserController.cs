using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;
using Papyrus.Shared.Models.Admin;

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

    [HttpGet("{id}")]
    public UserDTO Get(string id) => userService.GetMapped<UserDTO>(id);

    [HttpGet("{id}/Light")]
    public UserLightDTO GetLight(string id) => userService.GetMapped<UserLightDTO>(id);

    [HttpGet("{id}/Settings")]
    [Authorize(Roles = "Administration")]
    public UserSettingDTO GetSetting(string id) => userService.GetSettings(id);

    [HttpPut("{id}")]
    public void Update(string id, [FromBody] UserModel model) => userService.Update(id, model);

    [HttpGet("Current")]
    public UserDTO Current() => userService.GetCurrent<UserDTO>();

    [HttpGet("Exists")]
    public bool Exists([FromQuery] string userName, [FromQuery] string email) => userService.IsExist(userName, email, true);

    [HttpPut("Image")]
    public void UpdateImage([FromBody] ImageModel model) => userService.UpdateImage(model);

    [HttpPut("Password")]
    public async Task UpdatePassword([FromBody] UserPasswordModel model) => await userService.UpdatePassword(model);

    [HttpPut("{id}/Settings")]
    [Authorize(Roles = "Administration")]
    public void UpdateSettings(string id, [FromBody] UserSettingModel model) => userService.UpdateSettings(id, model);

    [HttpGet("Search")]
    public List<UserLightDTO> Search([FromQuery] string searchTerm, [FromQuery] bool ignoreCurrent, [FromQuery] List<string> ignored) => userService.Search(searchTerm, ignoreCurrent, ignored);
}

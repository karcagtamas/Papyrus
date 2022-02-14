using Microsoft.AspNetCore.Identity;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Enums;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Logic.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly IUserService userService;
    private readonly ITokenService tokenService;

    public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, IUserService userService,
        ITokenService tokenService)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.userService = userService;
        this.tokenService = tokenService;
    }

    public async Task<TokenDTO> Login(LoginModel model)
    {
        var user = userManager.Users.SingleOrDefault(u => u.UserName == model.UserName);

        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        if (user.Disabled)
        {
            throw new ArgumentException("User is disabled");
        }

        if (!await userManager.CheckPasswordAsync(user, model.Password))
            throw new ArgumentException("Incorrect username or password");

        user.LastLogin = DateTime.Now;
        userService.Update(user);
        userService.Persist();

        return await CreateTokensAndSave(user, Guid.NewGuid().ToString());
    }

    public async Task Register(RegistrationModel model)
    {
        if (userService.IsExist(model.UserName, model.Email))
        {
            throw new ArgumentException("User already created");
        }

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            FullName = model.FullName,
            Email = model.Email,
            LastLogin = DateTime.Now,
            Registration = DateTime.Now,
            Disabled = false
        };

        var result = await userManager.CreateAsync(user, model.Password);

        await AddDefaultRoleByResult(result, user);
    }

    public void Logout(string userName, string clientId)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new ArgumentException("User is not logged in");
        }

        var user = userService.GetByName(userName);

        if (user is null)
        {
            throw new ArgumentException("User does not exist");
        }

        foreach (var token in user.RefreshTokens)
        {
            if (token.IsActive && token.ClientId == clientId)
            {
                token.Revoked = DateTime.Now;
            }
        }

        userService.Update(user);
        userService.Persist();
    }

    private async Task AddDefaultRoleByResult(IdentityResult result, User user)
    {
        if (!result.Succeeded)
        {
            throw new ArgumentException(result.Errors.FirstOrDefault()?.Description);
        }
        else
        {
            if (await roleManager.RoleExistsAsync(ServerRole.User.ToString()))
            {
                await userManager.AddToRoleAsync(user, ServerRole.User.ToString());
            }
        }
    }

    private async Task<TokenDTO> CreateTokensAndSave(User user, string clientId)
    {
        var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive && t.ClientId == clientId);
        if (refreshToken is null)
        {
            refreshToken = tokenService.BuildRefreshToken(clientId);
            user.RefreshTokens.Add(refreshToken);
            userService.Update(user);
            userService.Persist();
        }

        return new TokenDTO
        {
            AccessToken = tokenService.BuildAccessToken(userService.GetMapped<UserTokenDTO>(user.Id), await userManager.GetRolesAsync(user)),
            RefreshToken = refreshToken.Token,
            ClientId = clientId,
            UserId = user.Id
        };
    }
}

using System.Security.Cryptography;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Authentication.JWT;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Auth.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Auth;
using RefreshToken = Papyrus.DataAccess.Entities.RefreshToken;

namespace Papyrus.Logic.Services.Auth;

public class TokenService : ITokenService
{
    private readonly IUserService userService;
    private readonly UserManager<User> userManager;
    private readonly IJWTAuthService jwtAuthService;

    public TokenService(IUserService userService, UserManager<User> userManager, IJWTAuthService jwtAuthService)
    {
        this.userService = userService;
        this.userManager = userManager;
        this.jwtAuthService = jwtAuthService;
    }

    public string BuildAccessToken(UserTokenDTO user, IList<string> roles) => jwtAuthService.BuildAccessToken(new UserEntity { Id = user.Id, UserName = user.UserName, Email = user.Email }, roles).Token;

    public RefreshToken BuildRefreshToken(string clientId)
    {
        var token = jwtAuthService.BuildRefreshToken();
        return new RefreshToken
        {
            Token = token.Token,
            Expires = token.Expires,
            Created = token.Creation,
            ClientId = clientId
        };
    }

    public async Task<TokenDTO> Refresh(string refreshToken, string clientId)
    {
        ExceptionHelper.Throw(string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(refreshToken), "Input model is invalid");

        User user = ObjectHelper.OrElseThrow(userService.GetByRefreshToken(refreshToken, clientId), () => new ServerException("The refresh token is invalid"));

        ExceptionHelper.Throw(user.Disabled, "User is disabled");

        var oldRefreshToken = user.RefreshTokens.Single(t => t.Token == refreshToken && t.ClientId == clientId);

        ExceptionHelper.Check(oldRefreshToken.IsActive, "The refresh token is expired");

        oldRefreshToken.Revoked = DateTime.Now;

        user.RefreshTokens.Where(x => x.ClientId == clientId)
            .ToList()
            .ForEach(token => token.Revoked = DateTime.Now);

        var newRefreshToken = BuildRefreshToken(clientId);
        user.RefreshTokens.Add(newRefreshToken);
        userService.Update(user);
        userService.Persist();

        var accessToken = BuildAccessToken(userService.GetMapped<UserTokenDTO>(user.Id),
            await userManager.GetRolesAsync(userManager.Users.First(u => u.Id == user.Id)));

        return new TokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            ClientId = clientId,
            UserId = user.Id
        };
    }

    public class UserEntity : IUser
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}

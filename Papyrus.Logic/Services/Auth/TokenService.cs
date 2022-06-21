using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KarcagS.Common.Tools.HttpInterceptor;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Configurations;
using Papyrus.Logic.Services.Auth.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Auth;

namespace Papyrus.Logic.Services.Auth;

public class TokenService : ITokenService
{
    private readonly JWTConfiguration jwtConfigurations;
    private readonly IUserService userService;
    private readonly UserManager<User> userManager;

    public TokenService(IOptions<JWTConfiguration> jwtOptions, IUserService userService, UserManager<User> userManager)
    {
        jwtConfigurations = jwtOptions.Value;
        this.userService = userService;
        this.userManager = userManager;
    }

    public string BuildAccessToken(UserTokenDTO user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        claims.AddRange(roleClaims);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(jwtConfigurations.Issuer, jwtConfigurations.Issuer, claims,
            expires: DateTime.Now.AddMinutes(jwtConfigurations.ExpirationInMinutes),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public RefreshToken BuildRefreshToken(string clientId)
    {
        var randomNumber = new byte[32];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.Now.AddDays(3),
            Created = DateTime.Now,
            ClientId = clientId
        };
    }

    public async Task<TokenDTO> Refresh(string refreshToken, string clientId)
    {
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("Input model is invalid");
        }

        var user = userService.GetByRefreshToken(refreshToken, clientId);

        if (user is null)
        {
            throw new ServerException("The refresh token is invalid");
        }

        if (user.Disabled)
        {
            throw new ServerException("User is disabled");
        }

        var oldRefreshToken = user.RefreshTokens.Single(t => t.Token == refreshToken && t.ClientId == clientId);

        if (!oldRefreshToken.IsActive)
        {
            throw new ServerException("The refresh token is expired");
        }

        oldRefreshToken.Revoked = DateTime.Now;

        user.RefreshTokens.Where(x => x.ClientId == clientId).ToList().ForEach(token => token.Revoked = DateTime.Now);

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
}
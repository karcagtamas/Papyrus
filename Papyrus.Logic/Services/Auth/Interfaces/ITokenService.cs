using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs.Auth;

namespace Papyrus.Logic.Services.Auth.Interfaces;

public interface ITokenService
{
    string BuildAccessToken(UserTokenDTO user, IList<string> roles);
    RefreshToken BuildRefreshToken(string clientId);
    Task<TokenDTO> Refresh(string refreshToken, string clientId);
}

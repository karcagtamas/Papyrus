using NoteWeb.DataAccess.Entities;
using NoteWeb.Shared.DTOs;

namespace NoteWeb.Logic.Services.Interfaces;

public interface ITokenService
{
    string BuildAccessToken(UserTokenDTO user, IList<string> roles);
    RefreshToken BuildRefreshToken(string clientId);
    Task<TokenDTO> Refresh(string refreshToken, string clientId);
}

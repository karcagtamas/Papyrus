using Papyrus.Shared.DTOs.Auth;

namespace Papyrus.Client.Services.Auth.Interfaces;

public interface ITokenService
{
    Task<TokenDTO> GetUser();
    Task RefreshStore();
    Task SetUser(TokenDTO dto);
    Task ClearUser();
    bool UserInStore();
    Task<string> GetClientId();
    Task<string> GetAccessToken();
}

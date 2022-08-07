using Papyrus.Shared.Models.Auth;

namespace Papyrus.Client.Services.Auth.Interfaces;

public interface IAuthService
{
    Task<string?> Login(LoginModel model);
    Task<bool> Register(RegistrationModel model);
    void Logout(string? redirectUri = null);
    void NotAuthorized();
    void Authorized();
    bool IsLoggedIn();
}

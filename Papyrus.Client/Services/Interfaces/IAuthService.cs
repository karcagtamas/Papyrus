using Papyrus.Shared.Models;

namespace Papyrus.Client.Services.Interfaces;

public interface IAuthService
{
    Task<string?> Login(LoginModel model);
    void Logout();
    void NotAuthorized();
    bool IsLoggedIn();
}

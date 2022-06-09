using Papyrus.Shared.DTOs.Auth;
using Papyrus.Shared.Models.Auth;

namespace Papyrus.Logic.Services.Auth.Interfaces;

public interface IAuthService
{
    Task<TokenDTO> Login(LoginModel model);
    Task Register(RegistrationModel model);
    void Logout(string clientId);
}

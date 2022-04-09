using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Logic.Services.Interfaces;

public interface IAuthService
{
    Task<TokenDTO> Login(LoginModel model);
    Task Register(RegistrationModel model);
    void Logout(string clientId);
}

using NoteWeb.Shared.DTOs;
using NoteWeb.Shared.Models;

namespace NoteWeb.Logic.Services.Interfaces;

public interface IAuthService
{
    Task<TokenDTO> Login(LoginModel model);
    void Logout(string userName, string clientId);
}

using Microsoft.AspNetCore.Components.Authorization;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO?> Current(AuthenticationState state);
    Task<bool> Exists(string userName, string email);
}

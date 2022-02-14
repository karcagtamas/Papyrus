using Karcags.Blazor.Common.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Client.Services.Interfaces;

public interface IUserService : IHttpCall<string>
{
    Task<UserDTO> Current();
    Task<bool> Exists(string userName, string email);
    Task<bool> SetDisableStatus(List<string> ids, bool status);
    Task<bool> UpdateImage(byte[] image);
    Task<bool> UpdatePassword(UserPasswordModel model);
}

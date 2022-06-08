using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Client.Services.Interfaces;

public interface IUserService : IHttpCall<string>
{
    Task<UserDTO?> Current();
    Task<UserLightDTO?> Light(string id);
    Task<bool> Exists(string userName, string email);
    Task<bool> SetDisableStatus(List<string> ids, bool status);
    Task<bool> UpdateImage(byte[] image);
    Task<bool> UpdatePassword(UserPasswordModel model);
}

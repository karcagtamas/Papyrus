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
    Task<bool> UpdateImage(ImageModel model);
    Task<bool> UpdatePassword(UserPasswordModel model);
    Task<List<UserLightDTO>> Search(string searchTerm, bool ignoreCurrent, List<string> ignored);
}

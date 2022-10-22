using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models;
using Papyrus.Shared.Models.Admin;

namespace Papyrus.Client.Services.Interfaces;

public interface IUserService : IHttpCall<string>
{
    Task<UserDTO?> Current();
    Task<UserLightDTO?> Light(string id);
    Task<UserSettingDTO?> GetSettings(string id);
    Task<bool> Exists(string userName, string email);
    Task<bool> UpdateImage(ImageModel model);
    Task<bool> UpdatePassword(UserPasswordModel model);
    Task<bool> UpdateSettings(string id, UserSettingModel model);
    Task<List<UserLightDTO>> Search(string searchTerm, bool ignoreCurrent, List<string> ignored);
    Task<List<AccessDTO>> GetAppAccesses();
    Task<List<NoteListDTO>> GetRecentNoteAccesses();
    Task<List<NoteListDTO>> GetMostCommonNoteAccesses();
}

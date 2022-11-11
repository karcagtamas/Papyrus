using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;
using Papyrus.Shared.Models.Admin;

namespace Papyrus.Logic.Services.Interfaces;

public interface IUserService : IMapperRepository<User, string>
{
    User? GetByName(string userName);
    T? GetMappedByName<T>(string userName);
    User? GetByEmail(string email);
    User? GetByRefreshToken(string token, string clientId);
    T GetCurrent<T>();
    UserSettingDTO GetSettings(string id);
    Task<bool> IsAdministrator();
    Task<bool> IsUserAdministrator(User user);
    bool IsExist(string userName, string email, bool ignoreCurrent);
    void UpdateImage(ImageModel model);
    Task UpdatePassword(UserPasswordModel model);
    void Update(string id, UserModel model);
    void UpdateSettings(string id, UserSettingModel model);
    List<UserLightDTO> Search(string searchTerm, bool ignoreCurrent, List<string> ignored);
    List<AccessDTO> GetAppAccesses();
}

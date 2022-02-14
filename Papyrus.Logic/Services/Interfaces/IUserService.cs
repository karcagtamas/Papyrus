using Karcags.Common.Tools.Repository;
using Papyrus.DataAccess.Entities;
using Papyrus.Shared.Models;

namespace Papyrus.Logic.Services.Interfaces;

public interface IUserService : IMapperRepository<User, string>
{
    User GetByName(string userName);
    T GetMappedByName<T>(string userName);
    User GetByEmail(string email);
    User GetByRefreshToken(string token, string clientId);
    T GetCurrent<T>();
    bool IsExist(string userName, string email);
    void SetDisableStatus(UserDisableStatusModel statusModel);
    void UpdateImage(byte[] image);
    void UpdatePassword(UserPasswordModel model);
}

using Karcags.Common.Tools.Repository;
using NoteWeb.DataAccess.Entities;

namespace NoteWeb.Logic.Services.Interfaces;

public interface IUserService : IMapperRepository<User, string>
{
    User GetByName(string userName);
    T GetMappedByName<T>(string userName);
    User GetByEmail(string email);
    User GetByRefreshToken(string token, string clientId);
    bool IsExist(string userName, string email);
}

using AutoMapper;
using Karcags.Common.Tools.Repository;
using Karcags.Common.Tools.Services;
using Microsoft.AspNetCore.Identity;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Logic.Services;

public class UserService : MapperRepository<User, string>, IUserService
{
    private readonly UserManager<User> userManager;
    public UserService(NoteWebContext context, ILoggerService logger, IUtilsService utils, IMapper mapper, UserManager<User> userManager) : base(context, logger, utils, mapper, "User")
    {
        this.userManager = userManager;
    }

    public User GetByEmail(string email)
    {
        return userManager.Users.SingleOrDefault(user => user.Email == email);
    }

    public User GetByName(string userName)
    {
        return userManager.Users.SingleOrDefault(user => user.UserName == userName);
    }

    public User GetByRefreshToken(string token, string clientId)
    {
        return GetList(x => x.RefreshTokens.Any(t => t.Token == token && t.ClientId == clientId)).FirstOrDefault();
    }

    public T GetMappedByName<T>(string userName)
    {
        return Mapper.Map<T>(GetByName(userName));
    }

    public bool IsExist(string userName, string email)
    {
        return userManager.Users.Any(user => user.Email == email || user.UserName == userName);
    }
}

using System.Globalization;
using AutoMapper;
using Karcags.Common.Tools.Repository;
using Karcags.Common.Tools.Services;
using Microsoft.AspNetCore.Identity;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.Models;

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

    public T GetCurrent<T>()
    {
        return GetMapped<T>(Utils.GetCurrentUserId<string>());
    }

    public bool IsExist(string userName, string email)
    {
        return userManager.Users.Any(user => user.Email == email || user.UserName == userName);
    }

    public void SetDisableStatus(UserDisableStatusModel statusModel)
    {
        var list = GetList(x => statusModel.Ids.Contains(x.Id)).ToList();

        list.ForEach(x =>
        {
            x.Disabled = statusModel.Status;
            Update(x);
        });

        Persist();
    }

    public void UpdateImage(byte[] image)
    {
        var user = Utils.GetCurrentUser<User, string>();
        user.ImageData = image;
        user.ImageTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        Update(user);
        Persist();
    }

    public async void UpdatePassword(UserPasswordModel model)
    {
        var user = Utils.GetCurrentUser<User, string>();
        if (await userManager.CheckPasswordAsync(user, model.OldPassword))
        {
            if (model.NewPassword == model.OldPassword)
            {
                throw new ArgumentException("Passwords must be different");
            }

            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                throw new ArgumentException("Invalid password");
            }
        }
        else
        {
            throw new ArgumentException("Invalid old password");
        }
    }
}

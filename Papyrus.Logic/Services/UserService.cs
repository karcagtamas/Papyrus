using AutoMapper;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeMapping;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Common.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Mongo.DataAccess.Enums;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models;
using Papyrus.Shared.Models.Admin;

namespace Papyrus.Logic.Services;

public class UserService : MapperRepository<User, string, string>, IUserService
{
    private readonly UserManager<User> userManager;
    private readonly IFileService fileService;

    public UserService(PapyrusContext context, ILoggerService logger, IUtilsService<string> utils, IMapper mapper, UserManager<User> userManager, IFileService fileService) : base(context, logger, utils, mapper, "User")
    {
        this.userManager = userManager;
        this.fileService = fileService;
    }

    public User? GetByEmail(string email) => userManager.Users.SingleOrDefault(user => user.Email == email);

    public User? GetByName(string userName) => userManager.Users.SingleOrDefault(user => user.UserName == userName);

    public User? GetByRefreshToken(string token, string clientId) => GetList(x => x.RefreshTokens.Any(t => t.Token == token && t.ClientId == clientId)).FirstOrDefault();

    public T? GetMappedByName<T>(string userName) => Mapper.Map<T>(GetByName(userName));

    public T GetCurrent<T>() => GetMapped<T>(Utils.GetCurrentUserId() ?? "");

    public bool IsExist(string userName, string email, bool ignoreCurrent)
    {
        return userManager.Users
            .Where(user => ignoreCurrent && user.Id != Utils.GetCurrentUserId())
            .Any(user => user.Email == email || user.UserName == userName);
    }

    public void UpdateImage(ImageModel model)
    {
        var user = Utils.GetCurrentUser<User>();

        var fileId = fileService.Insert(new Mongo.DataAccess.Entities.File
        {
            Category = FileCategory.Picture,
            Name = model.Name,
            Content = model.Content,
            Size = model.Size,
            Extension = model.Extension,
            MimeType = MimeUtility.GetMimeMapping(model.Name)
        });

        user.ImageId = fileId;
        Update(user);
    }

    public async Task UpdatePassword(UserPasswordModel model)
    {
        var user = Utils.GetCurrentUser<User>();
        if (await userManager.CheckPasswordAsync(user, model.OldPassword))
        {
            if (model.NewPassword == model.OldPassword)
            {
                throw new ServerException("Passwords must be different");
            }

            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                throw new ServerException("Invalid password");
            }
        }
        else
        {
            throw new ServerException("Invalid old password");
        }
    }

    public void Update(string id, UserModel model)
    {
        var user = Get(id);

        if (user is null)
        {
            throw new ServerException($"User not found");
        }

        if (user.UserName != model.UserName && ObjectHelper.IsNotNull(GetByName(user.UserName)))
        {
            throw new ServerException($"User already exists with this name: {model.UserName}");
        }

        if (user.Email != model.Email && ObjectHelper.IsNotNull(GetByEmail(model.Email)))
        {
            throw new ServerException($"User already exists with this e-mail address: {model.Email}");
        }

        UpdateByModel(id, model);
    }

    public List<UserLightDTO> Search(string searchTerm, bool ignoreCurrent, List<string> ignored)
    {
        var userId = Utils.GetCurrentUserId();
        var term = searchTerm.ToLower();
        return GetMappedList<UserLightDTO>(x => !x.Disabled && (!ignoreCurrent || x.Id != userId) && !ignored.Contains(x.Id) && (x.UserName.ToLower().Contains(searchTerm) || x.Email.ToLower().Contains(searchTerm) || (x.FullName != null && x.FullName.ToLower().Contains(searchTerm))), 5).ToList();
    }

    public UserSettingDTO GetSettings(string id)
    {
        var user = Get(id);

        return new UserSettingDTO
        {
            Id = id,
            Disabled = user.Disabled,
            RoleId = user.Roles.FirstOrDefault()?.RoleId ?? ""
        };
    }

    public void UpdateSettings(string id, UserSettingModel model)
    {
        var user = Get(id);

        if (user.Disabled != model.Disabled)
        {
            user.Disabled = model.Disabled;

            Update(user, false);
        }

        var roleId = user.Roles.FirstOrDefault()?.RoleId;

        if (roleId != model.RoleId)
        {
            user.Roles = new List<IdentityUserRole<string>>()
            {
                new() { RoleId = model.RoleId, UserId = user.Id }
            };

            Update(user, false);
        }

        Persist();
    }

    public async Task<bool> IsAdministrator()
    {
        var user = Utils.GetCurrentUser<User>();

        if (ObjectHelper.IsNull(user))
        {
            return false;
        }

        return await userManager.IsInRoleAsync(user, "Administrator");
    }

    public List<AccessDTO> GetAppAccesses() => Mapper.Map<List<AccessDTO>>(Utils.GetCurrentUser<User>().AppAccesses.OrderByDescending(x => x.Timestamp).Take(5).ToList());
}

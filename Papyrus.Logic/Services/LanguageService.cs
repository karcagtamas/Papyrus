using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Services;

public class LanguageService : MapperRepository<Language, int, string>, ILanguageService
{
    private readonly IUserService userService;

    public LanguageService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IUserService userService) : base(context, loggerService, utilsService, mapper, "Language")
    {
        this.userService = userService;
    }

    public LanguageDTO? GetUserLanguage()
    {
        var userId = Utils.GetCurrentUserId();

        if (ObjectHelper.IsNull(userId))
        {
            return null;
        }

        var user = userService.Get(userId);

        if (ObjectHelper.IsNull(user.LanguageId))
        {
            user.LanguageId = 1;
            userService.Update(user);
            return GetMapped<LanguageDTO>((int)user.LanguageId);
        }

        return GetMapped<LanguageDTO>((int)user.LanguageId);
    }

    public void SetUserLanguage(int id)
    {
        var user = Utils.GetCurrentUser<User>();

        user.LanguageId = id;
        userService.Update(user);
    }
}

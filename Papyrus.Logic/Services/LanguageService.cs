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
    private readonly ITranslationService translationService;
    private readonly string TranslationSegment = "Language";
    private readonly int DefaultId = 1;

    public LanguageService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IUserService userService, ITranslationService translationService) : base(context, loggerService, utilsService, mapper, "Language")
    {
        this.userService = userService;
        this.translationService = translationService;
    }

    public Language Default() => Get(DefaultId);

    public T DefaultMapped<T>() => GetMapped<T>(DefaultId);

    public List<LanguageDTO> GetAllTranslated(string? lang = null)
    {
        var languages = GetAll();
        string current;
        if (ObjectHelper.IsNull(lang))
        {
            current = GetUserLangOrDefault();
        }
        else
        {
            current = lang;
        }

        var translations = translationService.GetValues(TranslationSegment, current);

        return languages.Select(lang =>
        {
            var dto = Mapper.Map<LanguageDTO>(lang);

            var t = translations.Where(x => x.Key == lang.Name).FirstOrDefault()?.Value ?? lang.Name;
            dto.Name = t;

            return dto;
        }).ToList();
    }

    public string GetUserLangOrDefault() => GetUserLanguage()?.ShortName ?? Default().ShortName;

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
            user.LanguageId = DefaultId;
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

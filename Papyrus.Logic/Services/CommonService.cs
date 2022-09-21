using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Enums;

namespace Papyrus.Logic.Services;

public class CommonService : ICommonService
{
    private readonly ITranslationService translationService;
    private readonly ILanguageService languageService;
    private readonly IUtilsService<string> utils;
    private readonly IUserService userService;
    private readonly string ThemeSegment = "Theme";

    public CommonService(ITranslationService translationService, ILanguageService languageService, IUtilsService<string> utils, IUserService userService)
    {
        this.userService = userService;
        this.utils = utils;
        this.languageService = languageService;
        this.translationService = translationService;
    }

    public int GetUserTheme() => utils.GetCurrentUser<User>().Theme;

    public List<ThemeDTO> GetTranslatedThemeList(string? lang = null) => translationService.GetValues(ThemeSegment, languageService.GetLangOrUserLang(lang)).Select(x => new ThemeDTO { Key = int.Parse(x.Key), Text = x.Value }).ToList();

    public void SetUserTheme(int key)
    {
        if (key == (int)Theme.Dark || key == (int)Theme.Light)
        {
            var user = utils.GetCurrentUser<User>();
            user.Theme = key;
            userService.Update(user);
        }
    }
}

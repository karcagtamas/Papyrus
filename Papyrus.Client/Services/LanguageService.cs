using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services;

public class LanguageService : HttpCall<int>, ILanguageService
{
    public LanguageService(IHttpService http, IStringLocalizer<LanguageService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Language", "Language", localizer)
    {
    }

    public Task<LanguageDTO?> GetUserLanguage()
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "User"));

        return Http.Get<LanguageDTO>(settings).ExecuteWithResult();
    }

    public Task SetUserLanguage(int id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "User"));

        return Http.Put(settings, id).ExecuteWithResult();
    }
}

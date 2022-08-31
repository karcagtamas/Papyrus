using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services;

public class LanguageService : HttpCall<int>, ILanguageService
{
    public LanguageService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/Language", "Language")
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

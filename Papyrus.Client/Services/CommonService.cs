using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Store;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services;

public class CommonService : ICommonService
{
    private readonly IHttpService httpService;
    private readonly IStoreService storeService;
    private readonly string url = $"{ApplicationSettings.BaseApiUrl}/Editor";

    public CommonService(IHttpService httpService, IStoreService storeService)
    {
        this.storeService = storeService;
        this.httpService = httpService;
    }

    public string GetFileUrl(string id) => $"{ApplicationSettings.BaseApiUrl}/File/{id}";

    public Task<List<ThemeDTO>> GetThemeList(string? lang = null)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("lang", lang);

        var settings = new HttpSettings(httpService.BuildUrl(url, "Theme", "Translated"))
            .AddQueryParams(queryParams);

        return httpService.Get<List<ThemeDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<int> GetUserTheme()
    {
        var settings = new HttpSettings(httpService.BuildUrl(url, "Theme", "User"));

        return httpService.GetInt(settings).ExecuteWithResultOrElse(0);
    }

    public async Task SetLocalTheme(int key)
    {
        await SetUserTheme(key);
        storeService.Add("theme", key);
    }

    public Task SetUserTheme(int key)
    {
        var settings = new HttpSettings(httpService.BuildUrl(url, "Theme", "User"));

        return httpService.Put<int>(settings, key).Execute();
    }
}

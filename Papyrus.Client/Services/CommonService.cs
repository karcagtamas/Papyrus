using Blazored.LocalStorage;
using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Store;
using Microsoft.JSInterop;
using Papyrus.Client.Services.Auth.Interfaces;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services;

public class CommonService : ICommonService
{
    private readonly IHttpService httpService;
    private readonly IStoreService storeService;
    private readonly ILocalStorageService localStorageService;
    private readonly IAuthService auth;
    private readonly string url = $"{ApplicationSettings.BaseApiUrl}/Common";

    public CommonService(IHttpService httpService, IStoreService storeService, ILocalStorageService localStorageService, IAuthService auth)
    {
        this.auth = auth;
        this.localStorageService = localStorageService;
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

    public async Task OpenNote(string id, IJSRuntime jsRuntime) => await jsRuntime.InvokeAsync<object>("open", $"/notes/editor/{id}", "_blank");

    public async Task SetLocalTheme(int key, bool post = true)
    {
        if (storeService.IsExists("theme"))
        {
            var current = storeService.Get<int>("theme");

            if (current == key)
            {
                return;
            }
        }

        if (post && auth.IsLoggedIn())
        {
            await SetUserTheme(key);
        }
        storeService.Add("theme", key);
        await localStorageService.SetItemAsync("uitheme", key);
    }

    public Task SetUserTheme(int key)
    {
        var settings = new HttpSettings(httpService.BuildUrl(url, "Theme", "User"));

        return httpService.Put<int>(settings, key).Execute();
    }
}

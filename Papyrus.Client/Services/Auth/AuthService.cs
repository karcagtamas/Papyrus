using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Papyrus.Client.Services.Auth.Interfaces;
using Papyrus.Shared.DTOs.Auth;
using Papyrus.Shared.Models.Auth;

namespace Papyrus.Client.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IHttpService httpService;
    private readonly NavigationManager navigationManager;
    private readonly AuthenticationStateProvider authenticationStateProvider;
    private readonly ITokenService tokenService;

    private string Url { get; set; } = ApplicationSettings.BaseApiUrl + "/Auth";

    public AuthService(IHttpService httpService, NavigationManager navigationManager,
        AuthenticationStateProvider authenticationStateProvider, ITokenService tokenService)
    {
        this.httpService = httpService;
        this.navigationManager = navigationManager;
        this.authenticationStateProvider = authenticationStateProvider;
        this.tokenService = tokenService;
    }

    public async Task<string?> Login(LoginModel model)
    {
        var settings = new HttpSettings(httpService.BuildUrl(Url, "Login")).AddToaster("Login");

        var user = await httpService.PostWithResult<TokenDTO, LoginModel>(settings, model).ExecuteWithResult();

        if (ObjectHelper.IsNull(user)) return null;

        await tokenService.SetUser(user);

        ((PapyrusAuthStateProvider)authenticationStateProvider).MarkUserAsAuthenticated();

        return user.UserId;
    }

    public Task<bool> Register(RegistrationModel model)
    {
        var settings = new HttpSettings(httpService.BuildUrl(Url, "Register"))
            .AddToaster("Registration");

        return httpService.Post(settings, model).Execute();
    }

    public bool IsLoggedIn() => tokenService.UserInStore();

    public async void Logout(string? redirectUri = null)
    {
        var settings = new HttpSettings(httpService.BuildUrl(Url, "Logout"));

        await httpService.Put(settings, await tokenService.GetClientId()).Execute();

        await tokenService.ClearUser();

        ((PapyrusAuthStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();


        var query = new Dictionary<string, string>();

        if (ObjectHelper.IsNotNull(redirectUri))
        {
            query.Add("redirectUri", redirectUri);
            navigationManager.NavigateTo(QueryHelpers.AddQueryString("/login", query));
        }
        else
        {
            navigationManager.NavigateTo(ObjectHelper.OrElse(redirectUri, "home"));
        }
    }

    public void NotAuthorized()
    {
        var query = new Dictionary<string, string>
        {
            ["redirectUri"] = string.IsNullOrEmpty(navigationManager.Uri) ? "dashboard" : navigationManager.ToBaseRelativePath(navigationManager.Uri)
        };

        navigationManager.NavigateTo(QueryHelpers.AddQueryString("/login", query));
    }

    public void Authorized() => navigationManager.NavigateTo("dashboard");
}

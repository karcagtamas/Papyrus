using Karcags.Blazor.Common.Http;
using Karcags.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Client.Services;

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

        var body = new HttpBody<LoginModel>(model);

        var user = await httpService.PostWithResult<TokenDTO, LoginModel>(settings, body);

        if (user == null) return null;

        await tokenService.SetUser(user);

        ((PapyrusAuthStateProvider)authenticationStateProvider).MarkUserAsAuthenticated();

        return user.UserId;
    }

    public async Task<bool> Register(RegistrationModel model)
    {
        var settings = new HttpSettings(httpService.BuildUrl(Url, "Register"))
            .AddToaster("Registration");

        var body = new HttpBody<RegistrationModel>(model);

        return await httpService.Post(settings, body);
    }

    public bool IsLoggedIn()
    {
        return tokenService.UserInStore();
    }

    public async void Logout()
    {
        await tokenService.ClearUser();

        ((PapyrusAuthStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
    }

    public void NotAuthorized()
    {
        var query = new Dictionary<string, string>
        {
            ["redirectUri"] = navigationManager.Uri
        };

        navigationManager.NavigateTo(QueryHelpers.AddQueryString("/login", query));
    }
}

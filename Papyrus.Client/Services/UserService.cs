using Karcags.Blazor.Common.Http;
using Karcags.Blazor.Common.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;
using static System.Net.WebRequestMethods;

namespace Papyrus.Client.Services;

public class UserService : HttpCall<string>, IUserService
{
    public UserService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/User", "User")
    {
    }

    public async Task<UserDTO?> Current(AuthenticationState state)
    {
        var user = state.User;

        if (user.Identity?.Name is null)
        {
            return null;
        }

        var queryParams = new HttpQueryParameters();
        queryParams.Add("user", user.Identity.Name);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Current")).AddQueryParams(queryParams);

        return await Http.Get<UserDTO>(settings);
    }

    public async Task<bool> Exists(string userName, string email)
    {
        var queryParams = new HttpQueryParameters();
        queryParams.Add("userName", userName);
        queryParams.Add("email", email);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists")).AddQueryParams(queryParams);

        return await Http.GetBool(settings);
    }
}

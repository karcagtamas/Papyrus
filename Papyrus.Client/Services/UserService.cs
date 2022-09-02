using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Client.Services;

public class UserService : HttpCall<string>, IUserService
{
    private readonly IStringLocalizer<UserService> localizer;

    public UserService(IHttpService http, IStringLocalizer<UserService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/User", "User", localizer)
    {
        this.localizer = localizer;
    }

    public Task<UserDTO?> Current()
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Current"));

        return Http.Get<UserDTO>(settings).ExecuteWithResult();
    }

    public Task<bool> Exists(string userName, string email)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("userName", userName)
            .Add("email", email);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists"))
            .AddQueryParams(queryParams);

        return Http.GetBool(settings).ExecuteWithResult();
    }

    public Task<UserLightDTO?> Light(string id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Light");

        var settings = new HttpSettings(Http.BuildUrl(Url))
            .AddPathParams(pathParams);

        return Http.Get<UserLightDTO>(settings).ExecuteWithResult();
    }

    public Task<List<UserLightDTO>> Search(string searchTerm, bool ignoreCurrent, List<string> ignored)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("searchTerm", searchTerm)
            .Add("ignoreCurrent", ignoreCurrent)
            .AddMultiple("ignored", ignored);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Search")).AddQueryParams(queryParams);

        return Http.Get<List<UserLightDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<bool> SetDisableStatus(List<string> ids, bool status)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Disable")).AddToaster(localizer["Toaster.Disable"]);

        return Http.Post(settings, new UserDisableStatusModel { Ids = ids, Status = status }).Execute();
    }

    public Task<bool> UpdateImage(ImageModel model)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Image")).AddToaster(localizer["Toaster.ImageUpdate"]);

        return Http.Put(settings, model).Execute();
    }

    public Task<bool> UpdatePassword(UserPasswordModel model)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Password")).AddToaster(localizer["Toaster.ImageUpdate"]);

        return Http.Put(settings, model).Execute();
    }
}

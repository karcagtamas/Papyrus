using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Client.Services;

public class UserService : HttpCall<string>, IUserService
{
    public UserService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/User", "User")
    {
    }

    public async Task<UserDTO?> Current()
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Current"));

        return await Http.Get<UserDTO>(settings).ExecuteWithResult();
    }

    public async Task<bool> Exists(string userName, string email)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("userName", userName)
            .Add("email", email);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists"))
            .AddQueryParams(queryParams);

        return await Http.GetBool(settings).ExecuteWithResult();
    }

    public async Task<UserLightDTO?> Light(string id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Light");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return await Http.Get<UserLightDTO>(settings).ExecuteWithResult();
    }

    public async Task<List<UserLightDTO>> Search(string searchTerm, bool ignoreCurrent, List<string> ignored)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("searchTerm", searchTerm)
            .Add("ignoreCurrent", ignoreCurrent)
            .AddMultiple("ignored", ignored);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Search")).AddQueryParams(queryParams);

        return await Http.Get<List<UserLightDTO>>(settings).ExecuteWithResult() ?? new();
    }

    public async Task<bool> SetDisableStatus(List<string> ids, bool status)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Disable")).AddToaster("Disable");

        return await Http.Post(settings, new UserDisableStatusModel { Ids = ids, Status = status }).Execute();
    }

    public async Task<bool> UpdateImage(ImageModel model)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Image")).AddToaster("Image Update");

        return await Http.Put(settings, model).Execute();
    }

    public async Task<bool> UpdatePassword(UserPasswordModel model)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Password")).AddToaster("Password Update");

        return await Http.Put(settings, model).Execute();
    }
}

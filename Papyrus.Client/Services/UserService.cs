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
        var queryParams = new HttpQueryParameters();
        queryParams.Add("userName", userName);
        queryParams.Add("email", email);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists")).AddQueryParams(queryParams);

        return await Http.GetBool(settings).ExecuteWithResult();
    }

    public async Task<bool> SetDisableStatus(List<string> ids, bool status)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Disable")).AddToaster("Disable");

        var body = new HttpBody<UserDisableStatusModel>(new UserDisableStatusModel { Ids = ids, Status = status });

        return await Http.Post(settings, body).Execute();
    }

    public async Task<bool> UpdateImage(byte[] image)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Image")).AddToaster("Image Update");

        var body = new HttpBody<byte[]>(image);

        return await Http.Put(settings, body).Execute();
    }

    public async Task<bool> UpdatePassword(UserPasswordModel model)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Password")).AddToaster("Password Update");

        var body = new HttpBody<UserPasswordModel>(model);

        return await Http.Put(settings, body).Execute();
    }
}

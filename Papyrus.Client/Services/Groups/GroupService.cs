using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups;

public class GroupService : HttpCall<int>, IGroupService
{
    public GroupService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/Group", "Group")
    {
    }

    public async Task<bool> Close(int id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Close"));

        return await Http.Put(settings, new HttpBody<object?>(null)).Execute();
    }

    public async Task<List<GroupListDTO>> GetUserList(bool hideClosed = false)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("hideClosed", hideClosed);

        var settings = new HttpSettings(Http.BuildUrl(Url, "User")).AddQueryParams(queryParams);

        return await Http.Get<List<GroupListDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<bool> IsClosable(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Closable");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return await Http.Get<bool>(settings).ExecuteWithResultOrElse(false);
    }
}

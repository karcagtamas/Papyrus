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

    public async Task<bool> Open(int id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Open"));

        return await Http.Put(settings, new HttpBody<object?>(null)).Execute();
    }

    public async Task<bool> Remove(int id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Remove"));

        return await Http.Put(settings, new HttpBody<object?>(null)).Execute();
    }

    public async Task<List<GroupListDTO>> GetUserList(bool hideClosed = false)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("hideClosed", hideClosed);

        var settings = new HttpSettings(Http.BuildUrl(Url, "User")).AddQueryParams(queryParams);

        return await Http.Get<List<GroupListDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<GroupRightsDTO> GetRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return await Http.Get<GroupRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<GroupRoleRightsDTO> GetRoleRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights")
            .Add("Role");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return await Http.Get<GroupRoleRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<GroupTagRightsDTO> GetTagRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights")
            .Add("Tag");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return await Http.Get<GroupTagRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<GroupMemberRightsDTO> GetMemberRights(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Rights")
            .Add("Member");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return await Http.Get<GroupMemberRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }
}

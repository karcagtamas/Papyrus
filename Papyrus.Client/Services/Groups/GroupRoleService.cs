using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups;

public class GroupRoleService : HttpCall<int>, IGroupRoleService
{
    public GroupRoleService(IHttpService http, IStringLocalizer<GroupRoleService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/GroupRole", "Group Role", localizer)
    {
    }

    public Task<bool> Exists(int groupId, string name)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("groupId", groupId)
            .Add("name", name);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists"))
            .AddQueryParams(queryParams);

        return Http.Get<bool>(settings).ExecuteWithResultOrElse(false);
    }

    public Task<List<GroupRoleDTO>> GetByGroup(int groupId, string? textFilter = null)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var queryParams = HttpQueryParameters.Build()
            .Add("textFilter", textFilter);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams)
            .AddQueryParams(queryParams);

        return Http.Get<List<GroupRoleDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<GroupRoleLightDTO>> GetLightByGroup(int groupId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId)
            .Add("Light");

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams);

        return Http.Get<List<GroupRoleLightDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

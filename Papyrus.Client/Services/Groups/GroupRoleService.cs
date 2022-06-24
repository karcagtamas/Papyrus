using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups;

public class GroupRoleService : HttpCall<int>, IGroupRoleService
{
    public GroupRoleService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/GroupRole", "Group Role")
    {
    }

    public async Task<bool> Exists(int groupId, string name)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("groupId", groupId)
            .Add("name", name);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists")).AddQueryParams(queryParams);

        return await Http.Get<bool>(settings).ExecuteWithResultOrElse(false);
    }

    public async Task<List<GroupRoleDTO>> GetByGroup(int groupId)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Group", groupId.ToString()));

        return await Http.Get<List<GroupRoleDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<List<GroupRoleLightDTO>> GetLightByGroup(int groupId)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Group", groupId.ToString(), "Light"));

        return await Http.Get<List<GroupRoleLightDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

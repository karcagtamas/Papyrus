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

    public async Task<List<GroupRoleDTO>> GetGroupList(int groupId)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Group", groupId.ToString()));

        return await Http.Get<List<GroupRoleDTO>>(settings).ExecuteWithResult() ?? new();
    }
}

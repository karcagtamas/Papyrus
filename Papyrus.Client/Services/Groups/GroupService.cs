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

    public async Task<List<GroupMemberDTO>> GetMembers(int groupId)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, groupId.ToString(), "Member"));

        return await Http.Get<List<GroupMemberDTO>>(settings).ExecuteWithResult() ?? new();
    }

    public async Task<List<GroupListDTO>> GetUserList()
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "User"));

        return await Http.Get<List<GroupListDTO>>(settings).ExecuteWithResult() ?? new();
    }
}

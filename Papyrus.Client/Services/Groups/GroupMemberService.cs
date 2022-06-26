using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups;

public class GroupMemberService : HttpCall<int>, IGroupMemberService
{
    public GroupMemberService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/GroupMember", "Group Member")
    {
    }

    public async Task<List<GroupMemberDTO>> GetByGroup(int groupId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group")).AddPathParams(pathParams);

        return await Http.Get<List<GroupMemberDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

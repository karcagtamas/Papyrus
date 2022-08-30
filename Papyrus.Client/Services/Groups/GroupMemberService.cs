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

    public Task<List<GroupMemberDTO>> GetByGroup(int groupId, string? textFilter = null)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var queryParams = HttpQueryParameters.Build()
            .Add("textFilter", textFilter);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams)
            .AddQueryParams(queryParams);

        return Http.Get<List<GroupMemberDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<string>> GetMemberKeys(List<int> memberIds)
    {
        var queryParams = HttpQueryParameters.Build()
            .AddMultiple("memberIds", memberIds);

        var settings = new HttpSettings(Http.BuildUrl(Url, "UserKeys"))
            .AddQueryParams(queryParams);

        return Http.Get<List<string>>(settings).ExecuteWithResultOrElse(new());
    }
}

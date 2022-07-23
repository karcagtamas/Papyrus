using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;

namespace Papyrus.Client.Services.Groups;

public class GroupActionLogService : HttpCall<long>, IGroupActionLogService
{
    public GroupActionLogService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/GroupActionLog", "Group Action Log")
    {
    }

    public Task<List<GroupActionLogDTO>> GetByGroup(int groupId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams);

        return Http.Get<List<GroupActionLogDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

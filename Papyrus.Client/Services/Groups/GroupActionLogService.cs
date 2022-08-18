using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Shared.Table;
using Papyrus.Client.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;

namespace Papyrus.Client.Services.Groups;

public class GroupActionLogService : HttpCall<long>, IGroupActionLogService
{
    public GroupActionLogService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/GroupActionLog", "Group Action Log")
    {
    }

    public Task<TableResult<GroupActionLogDTO>> GetByGroup(int groupId, int? page = null, int? size = null)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var queryParams = HttpQueryParameters.Build()
            .Add("page", page)
            .Add("size", size);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams)
            .AddQueryParams(queryParams);

        return Http.Get<TableResult<GroupActionLogDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

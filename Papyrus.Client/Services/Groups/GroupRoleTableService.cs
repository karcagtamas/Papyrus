using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using Papyrus.Client.Services.Groups.Interfaces;

namespace Papyrus.Client.Services.Groups;

public class GroupRoleTableService : TableService<int>, IGroupRoleTableService
{
    public GroupRoleTableService(IHttpService http) : base(http)
    {
    }

    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/GroupRoleTable";
}

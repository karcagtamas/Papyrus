using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using Papyrus.Client.Services.Interfaces;

namespace Papyrus.Client.Services;

public class UserTableService : TableService<string>, IUserTableService
{
    public UserTableService(IHttpService http) : base(http)
    {
    }

    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/UserTable";
}

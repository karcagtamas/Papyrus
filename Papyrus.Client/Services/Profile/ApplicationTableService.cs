using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using Papyrus.Client.Services.Profile.Interfaces;

namespace Papyrus.Client.Services.Profile;

public class ApplicationTableService : TableService<string>, IApplicationTableService
{
    public ApplicationTableService(IHttpService http) : base(http)
    {
    }

    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/ApplicationTable";
}

using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Profile.Interfaces;

namespace Papyrus.Client.Services.Profile;

public class ApplicationService : HttpCall<string>, IApplicationService
{
    public ApplicationService(IHttpService http, IStringLocalizer<ApplicationService>? localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Application", "Application", localizer)
    {
    }
}

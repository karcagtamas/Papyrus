using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Profile.Interfaces;

namespace Papyrus.Client.Services.Profile;

public class ApplicationService : HttpCall<string>, IApplicationService
{
    private readonly IStringLocalizer<ApplicationService> localizer;

    public ApplicationService(IHttpService http, IStringLocalizer<ApplicationService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Application", "Application", localizer)
    {
        this.localizer = localizer;
    }

    public Task<bool> RefreshSecret(string id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id, "RefreshSecret"))
            .AddToaster(localizer["Toaster.Refresh"]);

        return Http.PutWithoutBody(settings).Execute();
    }
}

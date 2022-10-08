using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services;

public class RoleService : HttpCall<string>, IRoleService
{
    public RoleService(IHttpService http, IStringLocalizer<RoleService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Role", "Role", localizer)
    {
    }

    public Task<List<RoleDTO>> GetAllTranslated(string? lang = null)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("lang", lang);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Translated"))
            .AddQueryParams(queryParams);

        return Http.Get<List<RoleDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

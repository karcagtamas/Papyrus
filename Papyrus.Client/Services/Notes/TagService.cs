using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Shared.Helpers;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Services.Notes;

public class TagService : HttpCall<int>, ITagService
{
    public TagService(IHttpService http, IStringLocalizer<TagService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Tag", "Tag", localizer)
    {
    }

    public Task<List<TagDTO>> GetByGroup(int groupId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group")).AddPathParams(pathParams);

        return Http.Get<List<TagDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<TagPathDTO> GetPath(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Path");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return Http.Get<TagPathDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<TagTreeItemDTO>> GetTree(int? groupId = null, int? filteredTag = null)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("filteredTag", filteredTag)
            .AddOptional("groupId", groupId, (v) => ObjectHelper.IsNotNull(groupId));

        var settings = new HttpSettings(Http.BuildUrl(Url, "Tree"))
            .AddQueryParams(queryParams);

        return Http.Get<List<TagTreeItemDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

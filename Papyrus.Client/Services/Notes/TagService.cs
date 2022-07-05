using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Shared.Helpers;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Services.Notes;

public class TagService : HttpCall<int>, ITagService
{
    public TagService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/Tag", "Tag")
    {
    }

    public async Task<List<TagDTO>> GetByGroup(int groupId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group")).AddPathParams(pathParams);

        return await Http.Get<List<TagDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<TagPathDTO> GetPath(int id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id)
            .Add("Path");

        var settings = new HttpSettings(Http.BuildUrl(Url)).AddPathParams(pathParams);

        return await Http.Get<TagPathDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<List<TagTreeItemDTO>> GetTree(int? groupId = null, int? filteredTag = null)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("filteredTag", filteredTag)
            .AddOptional("groupId", groupId, (v) => ObjectHelper.IsNotNull(groupId));

        var settings = new HttpSettings(Http.BuildUrl(Url, "Tree"))
            .AddQueryParams(queryParams);

        return await Http.Get<List<TagTreeItemDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

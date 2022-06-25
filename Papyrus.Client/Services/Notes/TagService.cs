using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Client.Services.Notes;

public class TagService : HttpCall<int>, ITagService
{
    public TagService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/Tag", "Tag")
    {
    }

    public async Task<List<GroupTagDTO>> GetByGroup(int groupId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group")).AddPathParams(pathParams);

        return await Http.Get<List<GroupTagDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<List<GroupTagTreeItemDTO>> GetTreeByGroup(int groupId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId)
            .Add("Tree");

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group")).AddPathParams(pathParams);

        return await Http.Get<List<GroupTagTreeItemDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

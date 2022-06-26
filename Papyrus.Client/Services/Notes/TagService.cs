using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Client.Services.Notes;

public class TagService : HttpCall<int>, ITagService
{
    public TagService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/Tag", "Tag")
    {
    }

    public async Task<bool> CreateGroupTag(GroupTagModel model)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"));

        var body = new HttpBody<GroupTagModel>(model);

        return await Http.Post(settings, body).Execute();
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
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Path"));

        return await Http.Get<TagPathDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<List<GroupTagTreeItemDTO>> GetTreeByGroup(int groupId, int? filteredTag = null)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId)
            .Add("Tree");

        var queryParams = HttpQueryParameters.Build()
            .Add("filteredTag", filteredTag);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams)
            .AddQueryParams(queryParams);

        return await Http.Get<List<GroupTagTreeItemDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<bool> UpdateGroupTag(int id, GroupTagModel model)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group")).AddPathParams(pathParams);

        var body = new HttpBody<GroupTagModel>(model);

        return await Http.Put(settings, body).Execute();
    }
}

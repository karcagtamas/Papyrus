using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Services.Notes;

public class NoteService : HttpCall<string>, INoteService
{
    public NoteService(IHttpService http) : base(http, $"{ApplicationSettings.BaseApiUrl}/Note", "Note")
    {
    }

    public Task<NoteCreationDTO?> CreateEmpty(int? groupId)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url)).AddToaster("Note created");

        return Http.PostWithResult<NoteCreationDTO, NoteCreateModel>(settings, new NoteCreateModel { GroupId = groupId }).ExecuteWithResult();
    }

    public Task<List<NoteLightDTO>> GetByGroup(int groupId, NoteSearchType searchType = NoteSearchType.All)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var queryParams = HttpQueryParameters.Build()
            .Add("searchType", searchType);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams)
            .AddQueryParams(queryParams);

        return Http.Get<List<NoteLightDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<NoteLightDTO>> GetByUser(NoteSearchType searchType = NoteSearchType.All)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("searchType", searchType);

        var settings = new HttpSettings(Http.BuildUrl(Url, "User"))
            .AddQueryParams(queryParams);

        return Http.Get<List<NoteLightDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<NoteLightDTO?> GetLight(string id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Light"))
            .AddPathParams(pathParams);

        return Http.Get<NoteLightDTO>(settings).ExecuteWithResult();
    }
}

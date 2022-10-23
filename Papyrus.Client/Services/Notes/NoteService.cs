using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Client.Utils;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Services.Notes;

public class NoteService : HttpCall<string>, INoteService
{
    private readonly IStringLocalizer<NoteService> localizer;

    public NoteService(IHttpService http, IStringLocalizer<NoteService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Note", "Note", localizer)
    {
        this.localizer = localizer;
    }

    public Task<bool> Access(string id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Access"));

        return Http.Post(settings, id).Execute();
    }

    public Task<NoteCreationDTO?> CreateEmpty(string folderId, int? groupId = null)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url))
            .AddToaster(localizer["Toaster.Create"]);

        return Http.PostWithResult<NoteCreationDTO, NoteCreateModel>(settings, new NoteCreateModel { GroupId = groupId, Title = localizer["NoteTitle"], FolderId = folderId })
            .ExecuteWithResult();
    }

    public Task<bool> Exists(string parentFolderId, string title, string? id)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("parentFolder", parentFolderId)
            .Add("title", title)
            .Add("id", id);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Exists"))
            .AddQueryParams(queryParams);

        return Http.GetBool(settings).ExecuteWithResult();
    }

    public Task<List<NoteLightDTO>> GetFiltered(NoteFilterQueryModel query, int? groupId)
    {
        var queryParams = HttpQueryParameters.Build()
            .AddNoteFilters(query)
            .Add("group", groupId);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Filtered"))
            .AddQueryParams(queryParams);

        return Http.Get<List<NoteLightDTO>>(settings)
            .ExecuteWithResultOrElse(new());
    }

    public Task<NoteLightDTO?> GetLight(string id)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(id);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Light"))
            .AddPathParams(pathParams);

        return Http.Get<NoteLightDTO>(settings).ExecuteWithResult();
    }

    public Task<NoteRightsDTO> GetRights(string id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Rights"));

        return Http.Get<NoteRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<SearchResultDTO>> Search(SearchQueryModel query)
    {
        var queryParams = HttpQueryParameters.Build()
            .AddSearchFilters(query);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Search"))
            .AddQueryParams(queryParams);

        return Http.Get<List<SearchResultDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<NoteDashboardDTO>> GetMostCommonNoteAccesses()
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Access", "Common"));

        return Http.Get<List<NoteDashboardDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<NoteDashboardDTO>> GetRecentNoteAccesses()
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, "Access", "Recent"));

        return Http.Get<List<NoteDashboardDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

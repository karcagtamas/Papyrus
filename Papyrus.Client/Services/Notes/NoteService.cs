using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Services.Notes;

public class NoteService : HttpCall<string>, INoteService
{
    private readonly IStringLocalizer<NoteService> localizer;

    public NoteService(IHttpService http, IStringLocalizer<NoteService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/Note", "Note", localizer)
    {
        this.localizer = localizer;
    }

    public Task<NoteCreationDTO?> CreateEmpty(int? groupId)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url))
            .AddToaster(localizer["Toaster.Create"]);

        return Http.PostWithResult<NoteCreationDTO, NoteCreateModel>(settings, new NoteCreateModel { GroupId = groupId, Title = localizer["NoteTitle"] })
            .ExecuteWithResult();
    }

    public Task<List<NoteLightDTO>> GetByGroup(int groupId, NotePublishType publishType = NotePublishType.All, bool archiveStatus = false)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(groupId);

        var queryParams = HttpQueryParameters.Build()
            .Add("publishType", publishType)
            .Add("archiveStatus", archiveStatus);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Group"))
            .AddPathParams(pathParams)
            .AddQueryParams(queryParams);

        return Http.Get<List<NoteLightDTO>>(settings).ExecuteWithResultOrElse(new());
    }

    public Task<List<NoteLightDTO>> GetByUser(NotePublishType publishType = NotePublishType.All, bool archiveStatus = false)
    {
        var queryParams = HttpQueryParameters.Build()
            .Add("publishType", publishType)
            .Add("archiveStatus", archiveStatus);

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

    public Task<NoteRightsDTO> GetRights(string id)
    {
        var settings = new HttpSettings(Http.BuildUrl(Url, id.ToString(), "Rights"));

        return Http.Get<NoteRightsDTO>(settings).ExecuteWithResultOrElse(new());
    }
}

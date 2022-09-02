using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using Microsoft.Extensions.Localization;
using Papyrus.Client.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes.ActionsLogs;

namespace Papyrus.Client.Services.Notes;

public class NoteActionLogService : HttpCall<long>, INoteActionLogService
{
    public NoteActionLogService(IHttpService http, IStringLocalizer<NoteActionLogService> localizer) : base(http, $"{ApplicationSettings.BaseApiUrl}/NoteActionLog", "Note Action Log", localizer)
    {
    }

    public Task<List<NoteActionLogDTO>> GetByNote(string noteId)
    {
        var pathParams = HttpPathParameters.Build()
            .Add(noteId);

        var settings = new HttpSettings(Http.BuildUrl(Url, "Note"))
            .AddPathParams(pathParams);

        return Http.Get<List<NoteActionLogDTO>>(settings).ExecuteWithResultOrElse(new());
    }
}

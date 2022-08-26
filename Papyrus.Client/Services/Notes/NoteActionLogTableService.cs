using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using Papyrus.Client.Services.Notes.Interfaces;

namespace Papyrus.Client.Services.Notes;

public class NoteActionLogTableService : TableService<long>, INoteActionLogTableService
{
    public NoteActionLogTableService(IHttpService http) : base(http)
    {
    }

    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/NoteActionLogTable";
}

using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Utils;

public static class Extensions
{
    public static HttpQueryParameters AddNoteFilters(this HttpQueryParameters queryParams, NoteFilterQueryModel query) 
    {
        return queryParams
            .Add("publishType", query.PublishType)
            .Add("archivedStatus", query.ArchivedStatus);
    }
}

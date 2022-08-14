using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Notes.ActionsLogs;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface INoteActionLogService : IHttpCall<long>
{
    Task<List<NoteActionLogDTO>> GetByNote(string noteId);
}

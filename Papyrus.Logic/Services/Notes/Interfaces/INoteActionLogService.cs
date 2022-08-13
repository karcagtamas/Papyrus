using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes.ActionsLogs;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteActionLogService : IMapperRepository<NoteActionLog, long>
{
    void AddActionLog(string note, string performer, NoteActionLogType type);
    List<NoteActionLogDTO> GetByNote(string noteId);
}

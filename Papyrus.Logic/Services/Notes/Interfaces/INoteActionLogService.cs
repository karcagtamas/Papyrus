using Papyrus.DataAccess.Entities;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteActionLogService
{
    void AddActionLog(string note, string performer, NoteActionLogType type, bool doPersist = false);
    IQueryable<ActionLog> GetQuery(string noteId);
}

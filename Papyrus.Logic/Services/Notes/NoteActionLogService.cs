using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes.ActionsLogs;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Logic.Services.Notes;

public class NoteActionLogService : MapperRepository<NoteActionLog, long, string>, INoteActionLogService
{
    public NoteActionLogService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Note Action Log")
    {
    }

    public void AddActionLog(string note, string performer, NoteActionLogType type)
    {
        var log = new NoteActionLog
        {
            NoteId = note,
            PerformerId = performer,
            Type = type,
            Text = GetText(type)
        };

        Create(log);
    }

    public List<NoteActionLogDTO> GetByNote(string noteId) => GetMappedList<NoteActionLogDTO>(x => x.NoteId == noteId)
            .OrderByDescending(x => x.Creation)
            .ToList();

    private static string GetText(NoteActionLogType type)
    {
        return type switch
        {
            NoteActionLogType.Create => "Note created",
            NoteActionLogType.TitleEdit => "Note title is edited",
            NoteActionLogType.ContentEdit => "Note content is edited",
            NoteActionLogType.TagEdit => "Tag(s) added or removed",
            NoteActionLogType.Delete => "Note deleted",
            NoteActionLogType.Publish => "Note publish status changed",
            _ => "",
        };
    }
}

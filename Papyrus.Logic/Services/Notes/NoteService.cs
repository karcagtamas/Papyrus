using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Services.Notes;

public class NoteService : MapperRepository<Note, string, string>, INoteService
{
    public NoteService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Note")
    {
    }

    public NoteCreationDTO CreateEmpty(int? groupId)
    {
        var note = new Note
        {
            Title = "New Document",
            Content = ""
        };

        if (ObjectHelper.IsNotNull(groupId))
        {
            note.GroupId = groupId;
        }
        else
        {
            var userId = Utils.GetRequiredCurrentUserId();
            note.UserId = userId;
        }

        var id = Create(note);

        return new NoteCreationDTO
        {
            Id = id,
            Title = note.Title
        };
    }

    public List<NoteLightDTO> GetByGroup(int groupId)
    {
        return GetMappedList<NoteLightDTO>(x => x.GroupId == groupId)
            .OrderByDescending(x => x.LastUpdate)
            .ToList();
    }

    public List<NoteLightDTO> GetByUser()
    {
        var userId = Utils.GetRequiredCurrentUserId();

        return GetMappedList<NoteLightDTO>(x => x.UserId == userId)
            .OrderByDescending(x => x.LastUpdate)
            .ToList();
    }
}

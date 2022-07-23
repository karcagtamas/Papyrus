using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
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

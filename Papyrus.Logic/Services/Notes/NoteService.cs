using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Groups;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes;

public class NoteService : MapperRepository<Note, string, string>, INoteService
{
    private readonly IGroupActionLogService groupActionLogService;

    public NoteService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupActionLogService groupActionLogService) : base(context, loggerService, utilsService, mapper, "Note")
    {
        this.groupActionLogService = groupActionLogService;
    }

    public NoteCreationDTO CreateEmpty(int? groupId)
    {
        var note = new Note
        {
            Title = "New Document",
            Content = ""
        };

        var userId = Utils.GetRequiredCurrentUserId();

        if (ObjectHelper.IsNotNull(groupId))
        {
            note.GroupId = groupId;
            groupActionLogService.AddActionLog((int)groupId, userId, GroupActionLogType.NoteCreate);
        }
        else
        {
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

    public void UpdateWithTags(string id, NoteModel model)
    {
        UpdateByModel(id, model);

        Note note = Get(id);

        var tags = note.Tags;
        tags = tags.Where(x => model.Tags.Any(t => t == x.TagId)).ToList();

        model.Tags.Where(x => !tags.Any(t => t.TagId == x)).ToList().ForEach(x =>
        {
            tags.Add(new NoteTag
            {
                NoteId = id,
                TagId = x
            });
        });

        note.Tags = tags;

        Update(note);
    }
}

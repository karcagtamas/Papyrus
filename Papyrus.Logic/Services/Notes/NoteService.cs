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
using Papyrus.Shared.Enums.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes;

public class NoteService : MapperRepository<Note, string, string>, INoteService
{
    private readonly IGroupActionLogService groupActionLogService;
    private readonly INoteActionLogService noteActionLogService;

    public NoteService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupActionLogService groupActionLogService, INoteActionLogService noteActionLogService) : base(context, loggerService, utilsService, mapper, "Note")
    {
        this.groupActionLogService = groupActionLogService;
        this.noteActionLogService = noteActionLogService;
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
    public override string Create(Note entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var id = base.Create(entity, doPersist);

        noteActionLogService.AddActionLog(id, userId, NoteActionLogType.Create);

        return id;
    }

    public List<NoteLightDTO> GetByGroup(int groupId, NoteSearchType searchType = NoteSearchType.All)
    {
        return GetMappedList<NoteLightDTO>(x => x.GroupId == groupId && !x.Deleted && (searchType == NoteSearchType.All || (searchType == NoteSearchType.Published && x.Public) || (searchType == NoteSearchType.NotPublished && !x.Public)))
            .OrderByDescending(x => x.LastUpdate)
            .ToList();
    }

    public List<NoteLightDTO> GetByUser(NoteSearchType searchType = NoteSearchType.All)
    {
        var userId = Utils.GetRequiredCurrentUserId();

        return GetMappedList<NoteLightDTO>(x => x.UserId == userId && !x.Deleted && (searchType == NoteSearchType.All || (searchType == NoteSearchType.Published && x.Public) || (searchType == NoteSearchType.NotPublished && !x.Public)))
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

    public override void Update(Note entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var original = Context.Entry(entity).OriginalValues;

        ObjectHelper.WhenNotNull(original, o =>
        {
            if (o["Title"] as string != entity.Title)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.TitleEdit);
            }

            if (Context.Entry(entity).Collection(x => x.Tags).IsModified)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.TagEdit);
            }

            if (o["Content"] as string != entity.Content)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.ContentEdit);
            }

            if (o["Public"] as bool? != entity.Public)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.Publish);
            }

            if (o["Deleted"] as bool? != entity.Deleted)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.Delete);
            }
        });

        base.Update(entity, doPersist);
    }

    public void Delete(string id)
    {
        var note = Get(id);

        note.Deleted = true;

        Update(note);
    }
}

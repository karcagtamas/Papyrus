using System.Linq.Expressions;
using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Mongo.DataAccess.Entities;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Groups;
using Papyrus.Shared.Enums.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes;

public class NoteService : MapperRepository<Note, string, string>, INoteService
{
    private readonly IGroupActionLogService groupActionLogService;
    private readonly INoteActionLogService noteActionLogService;
    private readonly INoteContentService noteContentService;
    private readonly IUserService userService;
    private readonly IGroupService groupService;
    private readonly IFolderService folderService;

    public NoteService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupActionLogService groupActionLogService, INoteActionLogService noteActionLogService, INoteContentService noteContentService, IUserService userService, IGroupService groupService, IFolderService folderService) : base(context, loggerService, utilsService, mapper, "Note")
    {
        this.folderService = folderService;
        this.groupActionLogService = groupActionLogService;
        this.noteActionLogService = noteActionLogService;
        this.noteContentService = noteContentService;
        this.userService = userService;
        this.groupService = groupService;
    }

    public NoteCreationDTO CreateEmpty(NoteCreateModel model)
    {
        var note = new Note
        {
            Title = model.Title,
            FolderId = model.FolderId,
        };

        var folder = folderService.Get(model.FolderId);

        var userId = Utils.GetRequiredCurrentUserId();

        ExceptionHelper.Check(ObjectHelper.IsNull(model.GroupId) ? userId == folder.UserId : model.GroupId == folder.GroupId, "Invalid Folder keys", "Server.Message.InvalidFolderKeys");

        if (ObjectHelper.IsNotNull(model.GroupId))
        {
            note.GroupId = model.GroupId;
            groupActionLogService.AddActionLog((int)model.GroupId, userId, GroupActionLogType.NoteCreate);
        }
        else
        {
            note.UserId = userId;
        }

        // Create content
        var contentId = noteContentService.Insert(new NoteContent { Content = "" });
        note.ContentId = contentId;

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

    public List<NoteLightDTO> GetFiltered(NoteFilterQueryModel query, int? groupId)
    {
        var userId = Utils.GetRequiredCurrentUserId();

        // TODO: Search by group

        return GetFilteredList(GetListAsQuery(x => x.UserId == userId), query);
    }

    public override T GetMapped<T>(string id)
    {
        var mapped = base.GetMapped<T>(id);

        if (mapped is NoteDTO dto)
        {
            if (ObjectHelper.IsNotNull(dto))
            {
                var content = noteContentService.Get(dto.ContentId);

                dto.Content = content?.Content ?? "";
            }
        }

        return mapped;
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

            if (o["ContentLastEdit"] as DateTime? != entity.ContentLastEdit)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.ContentEdit);
            }

            if (o["Public"] as bool? != entity.Public)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.Publish);
            }

            if (o["Archived"] as bool? != entity.Archived)
            {
                noteActionLogService.AddActionLog(entity.Id, userId, NoteActionLogType.Archived);
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

    public async Task<NoteRightsDTO> GetRights(string id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var isAdmin = await userService.IsAdministrator();
        var note = Get(id);

        if (isAdmin || ObjectHelper.IsNotNull(note.UserId) && note.UserId == userId)
        {
            return new NoteRightsDTO(true);
        }

        if (ObjectHelper.IsNotNull(note.GroupId))
        {
            var group = groupService.Get((int)note.GroupId);

            if (await groupService.HasFullAccess(group, userId))
            {
                return new NoteRightsDTO(true);
            }

            var role = groupService.GetGroupRole(group, userId);

            if (ObjectHelper.IsNotNull(role))
            {
                return new NoteRightsDTO
                {
                    CanView = role.ReadNote || role.EditNote || role.DeleteNote,
                    CanEdit = role.EditNote || role.DeleteNote,
                    CanDelete = role.DeleteNote,
                    CanViewLogs = role.ReadNoteActionLog
                };
            }
        }

        return new NoteRightsDTO();
    }

    private List<NoteLightDTO> GetFilteredList(IQueryable<Note> queryable, NoteFilterQueryModel query)
    {
        return Mapper.Map<List<NoteLightDTO>>(
            queryable
                .Where(x => !x.Deleted)
                .Where(x => query.PublishType == NotePublishType.All || (query.PublishType == NotePublishType.Published && x.Public) || (query.PublishType == NotePublishType.NotPublished && !x.Public))
                .Where(x => (query.ArchivedStatus && x.Archived) || (!query.ArchivedStatus && !x.Archived))
                .Where(x => query.TextFilter == null || x.Title.Contains(query.TextFilter) || (x.Creator != null && x.Creator.UserName.Contains(query.TextFilter)))
                .Where(x => query.DateFilter == null || x.Creation > query.DateFilter)
                .Where(x => query.Tags.Count == 0 || x.Tags.Any(t => query.Tags.Contains(t.TagId)))
                .OrderByDescending(x => x.LastUpdate)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Creator)
                .ToList()
            );
    }

    public List<SearchResultDTO> Search(SearchQueryModel query)
    {
        var userId = Utils.GetCurrentUserId();
        var resultQuery = SearchQuery(GetAllAsQuery().Include(x => x.Tags).ThenInclude(x => x.Tag), query, userId);

        var result = resultQuery.ToList();

        // Append additional notes
        if (query.IncludeContents)
        {
            var prevIds = result.Select(x => x.Id).ToList();
            var relevantNotes = GetRelevantNotes(userId)
                .Where(x => !prevIds.Contains(x.Id))
                .ToList();
            var contentSearchResult = noteContentService.Search(relevantNotes.Select(x => x.ContentId).ToList(), query.Text).Select(x => x.Id).ToList();
            result.AddRange(relevantNotes.Where(x => contentSearchResult.Contains(x.ContentId)).ToList());
        }

        return result.Select(x => new SearchResultDTO
        {
            NoteId = x.Id,
            DisplayTitle = x.Title,
            PublicStatus = x.Public,
            Creation = x.Creation,
            LastUpdate = x.LastUpdate,
            Archived = x.Archived,
            Tags = Mapper.Map<List<NoteTagDTO>>(x.Tags.Select(x => x.Tag).ToList()),
            Data = ConstructData(x, userId)
        })
        .OrderByDescending(x => x.Creation)
        .ToList();
    }

    private static SearchResultDataDTO ConstructData(Note note, string? userId)
    {
        // Determine category
        // Other by default => Public status
        var cat = SearchResultCategory.Other;
        bool openable = true;

        // User is logged in
        if (ObjectHelper.IsNotNull(userId))
        {
            // User is the owner
            if (note.UserId == userId)
            {
                cat = SearchResultCategory.User;
            }

            // Note is connected to a group and the user is a member
            if (ObjectHelper.IsNotNull(note.Group))
            {
                var member = note.Group.Members.FirstOrDefault(x => x.UserId == userId);

                ObjectHelper.WhenNotNull(member, m =>
                {
                    cat = SearchResultCategory.Group;
                    openable = m.Role.ReadNote || m.Role.EditNote || m.Role.DeleteNote;
                });
            }
        }

        // Determine owner of the note
        // Or a user or a group => One of the is always null
        string owner = string.Empty;
        bool isGroup = false;

        ObjectHelper.WhenNotNull(note.User, u => owner = u.UserName);
        ObjectHelper.WhenNotNull(note.Group, g =>
        {
            owner = g.Name;
            isGroup = true;
        });

        return new()
        {
            Type = cat,
            Owner = owner,
            OwnerIsGroup = isGroup,
            Openable = openable
        };
    }

    private IQueryable<Note> SearchQuery(IQueryable<Note> queryable, SearchQueryModel query, string? userId = null)
    {
        Expression<Func<Note, bool>> titlePredicate = x => x.Title.Contains(query.Text);
        Expression<Func<Note, bool>> tagNamePredicate = x => x.Tags.Any(x => x.Tag.Caption.Contains(query.Text));
        Expression<Func<Note, bool>> ownerPredicate = x => (x.User != null && x.User.UserName.Contains(query.Text)) || (x.Group != null && x.Group.Name.Contains(query.Text));

        queryable = queryable.AsExpandable().Where(x => titlePredicate.Invoke(x) || ownerPredicate.Invoke(x) || (query.IncludeTags && tagNamePredicate.Invoke(x)))
            .Include(x => x.User)
            .Include(x => x.Group)
#nullable disable
                .ThenInclude(x => x.Members)
                .ThenInclude(x => x.Role);

        if (ObjectHelper.IsNull(userId) || query.OnlyPublics)
        {
            queryable = queryable.Where(x => x.Public);
        }
        else
        {
            queryable = queryable.Where(x => x.Public || (x.User != null && x.UserId == userId) || (x.Group != null && x.Group.Members.Any(m => m.UserId == userId && (m.Role.ReadNoteList || m.Role.ReadNote || m.Role.EditNote || m.Role.DeleteNote))));
        }

        // Date interval checks
        if (query.StartDate != null && query.EndDate != null)
        {
            queryable = queryable.Where(x => x.Creation >= query.StartDate && x.Creation <= query.EndDate);
        }

        return queryable.Distinct();
    }

    private List<Note> GetRelevantNotes(string userId)
    {
        var query = GetListAsQuery(x => x.Public || (userId != null && (x.UserId == userId || (x.Group != null && x.Group.Members.Any(x => x.UserId == userId)))))
            .Include(x => x.Group)
            .ThenInclude(x => x.Members)
            .Distinct();

        return query.ToList();
    }
}

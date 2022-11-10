using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Repository;
using KarcagS.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Logic.Exceptions.External;
using Papyrus.Logic.Services.External.Interfaces;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Shared.DTOs.External;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Services.External;

public class ExternalService : IExternalService
{
    private readonly PapyrusContext context;
    private readonly IMapper mapper;
    private readonly IApplicationService applicationService;
    private readonly INoteService noteService;
    private readonly INoteContentService noteContentService;
    private readonly IGroupService groupService;

    public ExternalService(PapyrusContext context, IMapper mapper, IApplicationService applicationService, INoteService noteService, INoteContentService noteContentService, IGroupService groupService)
    {
        this.context = context;
        this.mapper = mapper;
        this.applicationService = applicationService;
        this.noteService = noteService;
        this.noteContentService = noteContentService;
        this.groupService = groupService;
    }

    public List<GroupListExtDTO> GetGroups(ApplicationQueryModel query)
    {
        return WithApplication<List<GroupListExtDTO>>(query, (app) =>
            context.Set<Group>().AsQueryable() // TODO: Not just owned?
                .Where(x => x.OwnerId == app.UserId)
                .ToList()
                .MapTo<GroupListExtDTO, Group>(mapper)
                .ToList());
    }

    public GroupExtDTO GetGroup(ApplicationQueryModel query, int id)
    {
        return WithGroup<GroupExtDTO>(query, id, (app, group) =>
        {
            // Check URLs => read rights
            return mapper.Map<GroupExtDTO>(group);
        });
    }

    public List<NoteExtDTO> GetNotes(ApplicationQueryModel query)
    {
        return WithApplication<List<NoteExtDTO>>(query, (app) =>
            context.Set<Note>().AsQueryable()
                .Where(x => x.UserId == app.UserId)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .ToList()
                .MapTo<NoteExtDTO, Note>(mapper)
                .ToList());
    }

    public NoteContentExtDTO GetNote(ApplicationQueryModel query, string id)
    {
        return WithApplication<NoteContentExtDTO>(query, (app) =>
        {
            var note = noteService.Get(id);

            ExceptionHelper.Check(note.UserId == app.UserId, "Note is not available", "External.Messages.NoteNotAvailable");

            var dto = mapper.Map<NoteContentExtDTO>(note);
            dto.Content = noteContentService.Get(note.ContentId)?.Content;

            return dto;
        });
    }

    public List<TagTreeExtDTO> GetTagsInTree(ApplicationQueryModel query)
    {
        return WithApplication<List<TagTreeExtDTO>>(query, (app) =>
            context.Set<Tag>().AsQueryable()
                .Where(x => x.UserId == app.UserId && x.ParentId == null)
                .ToList()
                .MapTo<TagTreeExtDTO, Tag>(mapper)
                .ToList());
    }

    public List<TagExtDTO> GetTagsInList(ApplicationQueryModel query)
    {
        return WithApplication<List<TagExtDTO>>(query, (app) =>
            context.Set<Tag>().AsQueryable()
                .Where(x => x.UserId == app.UserId)
                .ToList()
                .MapTo<TagExtDTO, Tag>(mapper)
                .ToList());
    }

    private T WithApplication<T>(ApplicationQueryModel query, Func<Application, T> func)
    {
        var app = applicationService.GetListAsQuery(x => x.PublicId == query.PublicId && x.SecretId == query.SecretId)
            .Include(x => x.User)
            .FirstOrDefault();

        if (ObjectHelper.IsNull(app))
        {
            throw new ApplicationNotFoundException();
        }

        return func(app);
    }

    private T WithGroup<T>(ApplicationQueryModel query, int groupId, Func<Application, Group, T> func)
    {
        return WithApplication<T>(query, (app) =>
        {
            var group = groupService.GetOptional(groupId);

            if (ObjectHelper.IsNull(group))
            {
                throw new GroupNotFoundException();
            }

            ExceptionHelper.Check(group.OwnerId == app.UserId, "Group is not available", "External.Messages.GroupNotAvailable");

            return func(app, group);
        });
    }
}

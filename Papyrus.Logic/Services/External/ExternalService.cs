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
using Papyrus.Logic.Utils;
using Papyrus.Shared.DTOs.External;
using Papyrus.Shared.Models.Profile;
using System.Linq.Expressions;

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

    public List<NoteExtDTO> GetNotes(ApplicationQueryModel query) => WithApplication<List<NoteExtDTO>>(query, (app) => FetchNotes(x => x.UserId == app.UserId, new BasicUrlBuilder()));

    public NoteContentExtDTO GetNote(ApplicationQueryModel query, string noteId)
    {
        return WithApplication<NoteContentExtDTO>(query, (app) =>
        {
            var note = noteService.Get(noteId);

            ExceptionHelper.Check(note.UserId == app.UserId, "Note is not available", "External.Messages.NoteNotAvailable");

            var dto = mapper.Map<NoteContentExtDTO>(note);
            dto.Content = noteContentService.Get(note.ContentId)?.Content;

            return dto;
        });
    }

    public List<T> GetTags<T>(ApplicationQueryModel query, bool inTree = false) where T : TagExtDTO => WithApplication<List<T>>(query, (app) => FetchTags<T>(x => x.UserId == app.UserId, inTree));

    public List<GroupListExtDTO> GetGroups(ApplicationQueryModel query)
    {
        return WithApplication<List<GroupListExtDTO>>(query, (app) =>
            context.Set<Group>().AsQueryable()
                .Where(x => x.OwnerId == app.UserId || x.Members.Any(m => m.UserId == app.UserId))
                .Include(x => x.Members)
                .ToList()
                .MapTo<GroupListExtDTO, Group>(mapper)
                .ToList());
    }

    public GroupExtDTO GetGroup(ApplicationQueryModel query, int groupId)
    {
        return WithGroup<GroupExtDTO>(query, groupId, (app, group) =>
        {
            // Check URLs => read rights
            return mapper.Map<GroupExtDTO>(group);
        });
    }

    public List<NoteExtDTO> GetGroupNotes(ApplicationQueryModel query, int groupId) => WithGroup<List<NoteExtDTO>>(query, groupId, (app, group) => FetchNotes(x => x.GroupId == group.Id, new GroupUrlBuilder(group.Id)));

    public NoteContentExtDTO GetGroupNote(ApplicationQueryModel query, int groupId, string noteId)
    {
        return WithGroup<NoteContentExtDTO>(query, groupId, (app, group) =>
        {
            var note = noteService.Get(noteId);

            ExceptionHelper.Check(note.GroupId == group.Id, "Note is not available", "External.Messages.NoteNotAvailable");

            var dto = mapper.Map<NoteContentExtDTO>(note);
            dto.Content = noteContentService.Get(note.ContentId)?.Content;

            return dto;
        });
    }

    public List<T> GetGroupTags<T>(ApplicationQueryModel query, int groupId, bool inTree = false) where T : TagExtDTO
    {
        return WithGroup<List<T>>(query, groupId, (app, group) =>
        {
            return FetchTags<T>(x => x.GroupId == group.Id, inTree);
        });
    }

    public List<GroupMemberExtDTO> GetGroupMembers(ApplicationQueryModel query, int groupId)
    {
        return WithGroup<List<GroupMemberExtDTO>>(query, groupId, (app, group) =>
        {
            return context.Set<GroupMember>().AsQueryable()
                .Where(x => x.GroupId == group.Id)
                .Include(x => x.Role)
                .Include(x => x.User)
                .ToList()
                .MapTo<GroupMemberExtDTO, GroupMember>(mapper)
                .ToList();
        });
    }

    /// <summary>
    /// Try to find an application by the given query data.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="query">Application Query</param>
    /// <param name="func">Executor function with application input</param>
    /// <returns>Result of the executor function</returns>
    /// <exception cref="ApplicationNotFoundException">When the application cannot be found</exception>
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

    /// <summary>
    /// Try to find a group with application
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="query">Application Query</param>
    /// <param name="groupId">Group identifier</param>
    /// <param name="func">Executor function with application and group input</param>
    /// <returns>Result of the executor function</returns>
    /// <exception cref="GroupNotFoundException">When the group cannot be found</exception>
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

    private List<NoteExtDTO> FetchNotes(Expression<Func<Note, bool>> expr, IExternalUrlBuilder urlBuilder)
    {
        return context.Set<Note>().AsQueryable()
               .Where(expr)
               .Include(x => x.Tags).ThenInclude(x => x.Tag)
               .ToList()
               .MapTo<NoteExtDTO, Note>(mapper)
               .Select(x =>
               {
                   x.Url = urlBuilder.Build($"Notes/{x.Id}");

                   return x;
               })
               .ToList();
    }

    private List<T> FetchTags<T>(Expression<Func<Tag, bool>> expr, bool inTree = false) where T : TagExtDTO
    {
        return context.Set<Tag>().AsQueryable()
                .Where(expr)
                .Where(x => !inTree || x.ParentId == null)
                .ToList()
                .MapTo<T, Tag>(mapper)
                .ToList();
    }
}

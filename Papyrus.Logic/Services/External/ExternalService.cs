using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Logic.Exceptions.Profile;
using Papyrus.Logic.Services.External.Interfaces;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Shared.DTOs.External;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Services.External;

public class ExternalService : IExternalService
{
    private readonly PapyrusContext context;
    private readonly IMapper mapper;
    private readonly IApplicationService applicationService;

    public ExternalService(PapyrusContext context, IMapper mapper, IApplicationService applicationService)
    {
        this.context = context;
        this.mapper = mapper;
        this.applicationService = applicationService;
    }

    public List<GroupExtDTO> GetGroups(ApplicationQueryModel query)
    {
        return WithApplication<List<GroupExtDTO>>(query, (app) =>
            context.Set<Group>().AsQueryable() // TODO: Not just owned?
                .Where(x => x.OwnerId == app.UserId)
                .ToList()
                .MapTo<GroupExtDTO, Group>(mapper)
                .ToList());
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

    private T WithApplication<T>(ApplicationQueryModel query, Func<Application, T> action)
    {
        var app = applicationService.GetListAsQuery(x => x.PublicId == query.PublicId && x.SecretId == query.SecretId)
            .Include(x => x.User)
            .FirstOrDefault();

        ObjectHelper.WhenNotNull(app, a => action(a));

        if (ObjectHelper.IsNull(app))
        {
            throw new ApplicationNotFoundException();
        }

        return action(app);
    }
}

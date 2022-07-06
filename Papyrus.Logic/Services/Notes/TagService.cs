using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.DataAccess.Enums.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Services.Notes;

public class TagService : MapperRepository<Tag, int, string>, ITagService
{
    private readonly IGroupActionLogService groupActionLogService;

    public TagService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupActionLogService groupActionLogService) : base(context, loggerService, utilsService, mapper, "Tag")
    {
        this.groupActionLogService = groupActionLogService;
    }

    public List<TagDTO> GetByGroup(int groupId) => GetMappedList<TagDTO>(x => x.GroupId == groupId).ToList();

    public TagPathDTO GetPath(int id)
    {
        return new TagPathDTO
        {
            Id = id,
            Path = GetPathString(id)
        };
    }

    public List<TagTreeItemDTO> GetTree(int? groupId, int? filteredTag = null)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var tags = GetList(x => ((groupId != null && x.GroupId == groupId) || x.UserId == userId) && x.ParentId == null && (filteredTag == null || x.Id != filteredTag)); ;

        return tags.Select(x => Map(x, filteredTag)).ToList();
    }

    public override int CreateFromModel<TModel>(TModel model, bool doPersist = true)
    {
        var entity = Mapper.Map<Tag>(model);

        ExceptionHelper.Throw(ObjectHelper.IsNotNull(entity.GroupId) && ObjectHelper.IsNotNull(entity.UserId), "User and Group connection cannot be null");

        if (ObjectHelper.IsNull(entity.GroupId) && ObjectHelper.IsNull(entity.UserId))
        {
            entity.UserId = Utils.GetRequiredCurrentUserId();
        }

        return Create(entity, doPersist);
    }

    public override void UpdateByModel<TModel>(int id, TModel model, bool doPersist = true)
    {
        var entity = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Entity not found"));
        var userId = entity.UserId;
        var groupId = entity.GroupId;

        Mapper.Map(model, entity);

        ExceptionHelper.Check(entity.UserId == userId && entity.GroupId == groupId, "User and Group connection cannot be changed");

        Update(entity, doPersist);
    }

    public override int Create(Tag entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var id = base.Create(entity, doPersist);

        if (ObjectHelper.IsNotNull(entity.GroupId))
        {
            groupActionLogService.AddActionLog((int)entity.GroupId, userId, GroupActionLogType.TagCreate);
        }

        return id;
    }

    public override void Update(Tag entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Update(entity, doPersist);

        if (ObjectHelper.IsNotNull(entity.GroupId))
        {
            groupActionLogService.AddActionLog((int)entity.GroupId, userId, GroupActionLogType.TagEdit);
        }
    }

    public override void Delete(Tag entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Delete(entity, doPersist);

        if (ObjectHelper.IsNotNull(entity.GroupId))
        {
            groupActionLogService.AddActionLog((int)entity.GroupId, userId, GroupActionLogType.TagRemove);
        }
    }

    public string GetPathString(int id)
    {
        var tag = Get(id);

        if (tag.ParentId is null)
        {
            return tag.Caption;
        }

        return $"{GetPathString((int)tag.ParentId)}/{tag.Caption}";
    }

    private TagTreeItemDTO Map(Tag tag, int? filteredTag = null)
    {
        return new TagTreeItemDTO
        {
            Id = tag.Id,
            Caption = tag.Caption,
            Description = tag.Description,
            Color = tag.Color,
            Children = tag.Children.Where(x => filteredTag == null || x.Id != filteredTag).Select(x => Map(x)).ToList(),
        };
    }
}

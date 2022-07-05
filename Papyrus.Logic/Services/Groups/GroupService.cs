using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupService : MapperRepository<Group, int, string>, IGroupService
{
    private readonly IGroupRoleService groupRoleService;
    private readonly IGroupMemberService groupMemberService;

    public GroupService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupRoleService groupRoleService, IGroupMemberService groupMemberService) : base(context, loggerService, utilsService, mapper, "Group")
    {
        this.groupRoleService = groupRoleService;
        this.groupMemberService = groupMemberService;
    }

    public List<GroupListDTO> GetUserList(bool hideClosed = false)
    {
        string userId = ObjectHelper.OrElseThrow(Utils.GetRequiredCurrentUserId(), () => new ServerException("User not found"));

        return GetMappedList<GroupListDTO>(x => x.OwnerId == userId && (!hideClosed || !x.IsClosed))
            .OrderBy(x => x.IsClosed)
            .ThenByDescending(x => x.Creation)
            .ToList();
    }

    public override int CreateFromModel<TModel>(TModel model, bool doPersist = true)
    {
        string userId = ObjectHelper.OrElseThrow(Utils.GetRequiredCurrentUserId(), () => new ServerException("User not found"));
        var id = base.CreateFromModel(model, doPersist);

        var result = groupRoleService.CreateDefaultRoles(id);
        var admin = ObjectHelper.OrElseThrow(result.FirstOrDefault(x => x.IsAdministration), () => new ServerException("Admin role not found"));

        // Add current user as administrator
        var member = new GroupMember
        {
            GroupId = id,
            UserId = userId,
            RoleId = admin.Id,
            Creation = DateTime.Now
        };
        groupMemberService.Create(member);

        return id;
    }

    public bool IsClosable(int id)
    {
        string userId = ObjectHelper.OrElseThrow(Utils.GetRequiredCurrentUserId(), () => new ServerException("User not found"));

        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        return IsClosableForUser(group, userId);
    }

    public void Close(int id)
    {
        string userId = ObjectHelper.OrElseThrow(Utils.GetRequiredCurrentUserId(), () => new ServerException("User not found"));

        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        ExceptionHelper.Check(IsClosableForUser(group, userId), "Not have permission to Close this group.");

        group.IsClosed = true;
        Update(group);
    }

    public GroupRightsDTO GetRights(int id)
    {
        string userId = ObjectHelper.OrElseThrow(Utils.GetRequiredCurrentUserId(), () => new ServerException("User not found"));

        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        return new GroupRightsDTO
        {
            CanClose = IsClosableForUser(group, userId),
            CanOpen = IsOpenableForUser(group, userId),
            CanRemove = IsRemovableForUser(group, userId)
        };
    }

    public void Open(int id)
    {
        string userId = ObjectHelper.OrElseThrow(Utils.GetRequiredCurrentUserId(), () => new ServerException("User not found"));

        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        ExceptionHelper.Check(IsOpenableForUser(group, userId), "Not have permission to Open this group.");

        group.IsClosed = false;
        Update(group);
    }

    public void Remove(int id)
    {
        string userId = ObjectHelper.OrElseThrow(Utils.GetRequiredCurrentUserId(), () => new ServerException("User not found"));

        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        ExceptionHelper.Check(IsRemovableForUser(group, userId), "Not have permission to Remove this group.");

        Delete(group);
    }
    public GroupTagRightsDTO GetTagRights(int id)
    {
        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        return new GroupTagRightsDTO
        {
            CanCreate = !group.IsClosed,
            CanEdit = !group.IsClosed,
            CanRemove = !group.IsClosed
        };
    }

    public GroupMemberRightsDTO GetMemberRights(int id)
    {
        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        return new GroupMemberRightsDTO
        {
            CanAdd = !group.IsClosed,
            CanEdit = !group.IsClosed
        };
    }

    public GroupRoleRightsDTO GetRoleRights(int id)
    {
        Group group = ObjectHelper.OrElseThrow(Get(id), () => new ServerException("Group not found"));

        return new GroupRoleRightsDTO
        {
            CanCreate = !group.IsClosed,
            CanEdit = !group.IsClosed
        };
    }

    private static bool IsClosableForUser(Group group, string userId) => group.OwnerId == userId && !group.IsClosed;
    private static bool IsOpenableForUser(Group group, string userId) => group.OwnerId == userId && group.IsClosed;
    private static bool IsRemovableForUser(Group group, string userId) => group.OwnerId == userId;

}

using AutoMapper;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
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
        var user = Utils.GetRequiredCurrentUserId();

        if (user is null)
        {
            throw new ServerException("User not found");
        }

        return GetMappedList<GroupListDTO>(x => x.OwnerId == user && (!hideClosed || !x.IsClosed))
            .OrderBy(x => x.IsClosed)
            .ThenByDescending(x => x.Creation)
            .ToList();
    }

    public override int CreateFromModel<TModel>(TModel model, bool doPersist = true)
    {
        var user = Utils.GetRequiredCurrentUserId();
        var id = base.CreateFromModel(model, doPersist);

        var result = groupRoleService.CreateDefaultRoles(id);
        var admin = result.FirstOrDefault(x => x.IsAdministration);

        if (admin is null)
        {
            throw new ServerException("Admin role not found");
        }

        // Add current user as administrator
        var member = new GroupMember
        {
            GroupId = id,
            UserId = user,
            RoleId = admin.Id,
            Creation = DateTime.Now
        };
        groupMemberService.Create(member);

        return id;
    }

    public bool IsClosable(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();

        if (userId is null)
        {
            throw new ServerException("User not found");
        }

        var group = Get(id);

        if (group is null)
        {
            throw new ServerException("Group not found");
        }

        return IsClosableForUser(group, userId);
    }

    public void Close(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();

        if (userId is null)
        {
            throw new ServerException("User not found");
        }

        var group = Get(id);

        if (group is null)
        {
            throw new ServerException("Group not found");
        }

        if (!IsClosableForUser(group, userId))
        {
            throw new ServerException("Not have permission to Close this group.");
        }

        group.IsClosed = true;
        Update(group);
    }

    private static bool IsClosableForUser(Group group, string userId)
    {
        return group.OwnerId == userId && !group.IsClosed;
    }
}

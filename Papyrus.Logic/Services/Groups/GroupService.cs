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

    public GroupService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupRoleService groupRoleService) : base(context, loggerService, utilsService, mapper, "Group")
    {
        this.groupRoleService = groupRoleService;
    }

    public List<GroupListDTO> GetUserList()
    {
        var user = Utils.GetRequiredCurrentUserId();

        if (user is null)
        {
            throw new ServerException("User not found");
        }

        return GetMappedList<GroupListDTO>(x => x.OwnerId == user)
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
        Context.Set<GroupMember>().Add(member);
        Context.SaveChanges();

        return id;
    }

    public List<GroupMemberDTO> GetMembers(int id)
    {
        var group = Get(id);

        return Mapper.Map<List<GroupMemberDTO>>(group.Members);
    }

    public void AddMember(int id, string memberId)
    {
        var userId = Utils.GetCurrentUserId();

        if (userId is null)
        {
            throw new ServerException("User not found");
        }

        var group = Get(id);

        if (group is null)
        {
            throw new ServerException("Group not found");
        }

        if (group.Members.Any(x => x.UserId == memberId))
        {
            throw new ServerException("User already has been added");
        }

        var role = group.Roles.FirstOrDefault(x => x.IsDefault);

        if (role is null)
        {
            throw new ServerException("Default role not found");
        }

        group.Members.Add(new GroupMember
        {
            GroupId = group.Id,
            UserId = memberId,
            RoleId = role.Id,
            Creation = DateTime.Now,
            AddedById = userId
        });

        Update(group);
    }

    public void RemoveMember(int id, int groupMemberId)
    {
        var member = Context.Set<GroupMember>().Find(groupMemberId);

        if (member is null)
        {
            throw new ServerException("Group member not found");
        }

        if (member.GroupId != id)
        {
            throw new ServerException("Invalid member removing");
        }

        Context.Set<GroupMember>().Remove(member);
        Context.SaveChanges();
    }
}

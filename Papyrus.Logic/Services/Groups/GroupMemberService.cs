using AutoMapper;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupMemberService : MapperRepository<GroupMember, int, string>, IGroupMemberService
{
    private readonly IGroupRoleService groupRoleService;
    private readonly IGroupActionLogService groupActionLogService;

    public GroupMemberService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupRoleService groupRoleService, IGroupActionLogService groupActionLogService) : base(context, loggerService, utilsService, mapper, "Group Member")
    {
        this.groupRoleService = groupRoleService;
        this.groupActionLogService = groupActionLogService;
    }

    public void CreateFromModelWithDefaultRole<T>(T model)
    {
        var userId = Utils.GetCurrentUserId();
        var member = Mapper.Map<GroupMember>(model);

        GroupRole role = ObjectHelper.OrElseThrow(groupRoleService.GetList(x => x.GroupId == member.GroupId && x.IsDefault).FirstOrDefault(), () => new ServerException("Default role not found"));

        member.RoleId = role.Id;
        member.AddedById = userId;
        Create(member);
    }

    public override int Create(GroupMember entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var id = base.Create(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.MemberAdd);

        return id;
    }

    public override void Update(GroupMember entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Update(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.MemberEdit);
    }

    public override void Delete(GroupMember entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Delete(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.MemberRemove);
    }

    public List<string> GetMemberKeys(List<int> memberIds) => GetList(x => memberIds.Contains(x.Id)).Select(x => x.UserId).ToList();
}

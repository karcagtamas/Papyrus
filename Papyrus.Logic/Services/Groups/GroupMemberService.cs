using AutoMapper;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Logic.Services.Groups;

public class GroupMemberService : MapperRepository<GroupMember, int, string>, IGroupMemberService
{
    private readonly IGroupRoleService groupRoleService;

    public GroupMemberService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupRoleService groupRoleService) : base(context, loggerService, utilsService, mapper, "Group Member")
    {
        this.groupRoleService = groupRoleService;
    }

    public void CreateFromModelWithDefaultRole<T>(T model)
    {
        var member = Mapper.Map<GroupMember>(model);

        var role = groupRoleService.GetList(x => x.GroupId == member.GroupId && x.IsDefault).FirstOrDefault();

        if (role is null)
        {
            throw new ServerException("Default role not found");
        }

        member.RoleId = role.Id;
        Create(member);
    }
}

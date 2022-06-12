using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;
using static Papyrus.Logic.Services.Groups.GroupRoleService;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupRoleService : IMapperRepository<GroupRole, int>
{
    List<RoleCreationResultItem> CreateDefaultRoles(int groupId);
    List<GroupRoleDTO> GetGroupList(int groupId);
}

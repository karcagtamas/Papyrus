using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupRoleService : IMapperRepository<GroupRole, int>
{
    void CreateDefaultRoles(int groupId);
    List<GroupRoleDTO> GetGroupList(int groupId);
}

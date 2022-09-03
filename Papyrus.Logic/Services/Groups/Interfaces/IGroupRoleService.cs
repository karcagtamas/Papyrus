using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;
using static Papyrus.Logic.Services.Groups.GroupRoleService;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupRoleService : IMapperRepository<GroupRole, int>
{
    List<RoleCreationResultItem> CreateDefaultRoles(int groupId);
    List<GroupRoleDTO> GetByGroup(int groupId, string? textFilter = null);
    List<GroupRoleDTO> GetTranslatedByGroup(int groupId, string? textFilter = null, string? lang = null);
    List<GroupRoleLightDTO> GetLightByGroup(int groupId);
    List<GroupRoleLightDTO> GetLightTranslatedByGroup(int groupId, string? lang = null);
    bool Exists(int groupId, string name);
}

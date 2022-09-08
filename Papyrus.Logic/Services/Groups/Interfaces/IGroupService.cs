using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupService : IMapperRepository<Group, int>
{
    List<GroupListDTO> GetUserList(bool hideClosed = false);
    GroupRightsDTO GetRights(int id);
    GroupTagRightsDTO GetTagRights(int id);
    GroupMemberRightsDTO GetMemberRights(int id);
    GroupRoleRightsDTO GetRoleRights(int id);
    GroupRole? GetUserRole(int id);
    void Close(int id);
    void Open(int id);
    void Remove(int id);
}

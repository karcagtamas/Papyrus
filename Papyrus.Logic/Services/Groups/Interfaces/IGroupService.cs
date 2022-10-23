using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupService : IMapperRepository<Group, int>
{
    List<GroupListDTO> GetUserList(bool hideClosed = false);
    Task<GroupPageRightsDTO> GetPageRights(int id);
    Task<GroupRightsDTO> GetRights(int id);
    Task<GroupTagRightsDTO> GetTagRights(int id);
    Task<GroupMemberRightsDTO> GetMemberRights(int id);
    Task<GroupRoleRightsDTO> GetRoleRights(int id);
    Task<GroupNoteRightsDTO> GetNoteRights(int id);
    GroupRole? GetUserRole(int id);
    bool IsCurrentOwner(int id);
    Task Close(int id);
    Task Open(int id);
    Task Remove(int id);
    GroupRole? GetGroupRole(Group group, string userId);
    Task<bool> HasFullAccess(Group group, string userId);
    List<GroupNoteListDTO> GetRecentEdits(int id);
}

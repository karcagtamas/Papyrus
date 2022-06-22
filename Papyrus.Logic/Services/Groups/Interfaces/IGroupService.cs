using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupService : IMapperRepository<Group, int>
{
    List<GroupListDTO> GetUserList();
    List<GroupMemberDTO> GetMembers(int id);
    void AddMember(int id, string memberId);
    void RemoveMember(int id, int groupMemberId);
    void EditMember(int id, int groupMemberId, GroupMemberModel model);
}

using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupService : IMapperRepository<Group, int>
{
    List<GroupListDTO> GetUserList();
    List<GroupMemberDTO> GetMembers(int id);
}

using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupService : IMapperRepository<Group, int>
{
    List<GroupListDTO> GetUserList();
}

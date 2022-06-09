using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupService : IMapperRepository<Group, int>
{
}

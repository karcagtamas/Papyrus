using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupMemberService : IMapperRepository<GroupMember, int>
{
    void CreateFromModelWithDefaultRole<T>(T model);
}

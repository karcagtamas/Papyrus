using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Enums.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupActionLogService : IMapperRepository<GroupActionLog, long>
{
    void AddActionLog(int group, string performer, GroupActionLogType type);
}

using Papyrus.DataAccess.Entities;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.Logic.Services.Groups.Interfaces;

public interface IGroupActionLogService
{
    void AddActionLog(int group, string performer, GroupActionLogType type, bool doPersist = false);
    IQueryable<ActionLog> GetQuery(int groupId);
}

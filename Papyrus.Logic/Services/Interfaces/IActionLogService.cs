using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities;

namespace Papyrus.Logic.Services.Interfaces;

public interface IActionLogService : IMapperRepository<ActionLog, long>
{
    void AddActionLog(string key, string segment, string type, string? performer, Dictionary<string, string> texts, bool doPersist = false);
}

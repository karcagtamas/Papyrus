using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.Logic.Services;

public class ActionLogService : MapperRepository<ActionLog, long, string>, IActionLogService
{
    public ActionLogService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Action Log")
    {
    }

    public void AddActionLog(string key, string segment, string type, string? performer, Dictionary<string, string> texts)
    {
        var creation = DateTime.Now;
        foreach (var text in texts)
        {
            var log = new ActionLog
            {
                Key = key,
                Segment = segment,
                Type = type,
                PerformerId = performer,
                Text = text.Value,
                Language = text.Key,
                Creation = creation
            };

            Create(log);
        }
    }
}

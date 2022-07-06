using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Enums.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Logic.Services.Groups;

public class GroupActionLogService : MapperRepository<GroupActionLog, long, string>, IGroupActionLogService
{
    public GroupActionLogService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Group Action Log")
    {
    }

    public void AddActionLog(int group, string performer, GroupActionLogType type)
    {
        var log = new GroupActionLog
        {
            GroupId = group,
            PerformerId = performer,
            Type = type,
            Text = GetText(type)
        };

        Create(log);
    }

    private static string GetText(GroupActionLogType type)
    {
        return type switch
        {
            GroupActionLogType.Create => "Group created",
            GroupActionLogType.Close => "Group closed",
            GroupActionLogType.Open => "Group opened",
            GroupActionLogType.RoleCreate => "Role created",
            GroupActionLogType.RoleEdit => "Role edited",
            GroupActionLogType.RoleRemove => "Role removed",
            GroupActionLogType.MemberAdd => "Member added",
            GroupActionLogType.MemberEdit => "Member edited",
            GroupActionLogType.MemberRemove => "Member removed",
            GroupActionLogType.TagCreate => "Tag created",
            GroupActionLogType.TagEdit => "Tag edited",
            GroupActionLogType.TagRemove => "Tag removed",
            GroupActionLogType.DataEdit => "Data edited",
            _ => "",
        };
    }
}

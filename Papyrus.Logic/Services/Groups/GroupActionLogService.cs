using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Table;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;
using Papyrus.Shared.Enums.Groups;

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

    public TableResult<GroupActionLogDTO> GetByGroup(int groupId, int? page = null, int? size = null)
    {
        return new TableResult<GroupActionLogDTO>
        {
            Items = GetMappedList<GroupActionLogDTO>(x => x.GroupId == groupId, size, page == null || size == null ? null : page * size)
            .OrderByDescending(x => x.Creation)
            .ToList(),
            AllItemCount = Count(x => x.GroupId == groupId)
        };
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
            GroupActionLogType.NoteCreate => "Note created",
            _ => "",
        };
    }
}

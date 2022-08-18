using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupRoleService : MapperRepository<GroupRole, int, string>, IGroupRoleService
{
    private readonly IGroupActionLogService groupActionLogService;

    public GroupRoleService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupActionLogService groupActionLogService) : base(context, loggerService, utilsService, mapper, "Group Role")
    {
        this.groupActionLogService = groupActionLogService;
    }

    public List<RoleCreationResultItem> CreateDefaultRoles(int groupId)
    {
        return new() {
            new RoleCreationResultItem
            {
                Id = CreateAdminRole(groupId),
                IsAdministration = true
            },
            new RoleCreationResultItem
            {
                Id = CreateModeratorRole(groupId),
                IsAdministration = false
            },
            new RoleCreationResultItem
            {
                Id = CreateDefaultRole(groupId),
                IsAdministration = false
            },
        };
    }

    public List<GroupRoleDTO> GetByGroup(int groupId, string? textFilter = null)
    {
        return GetMappedList<GroupRoleDTO>(x => x.GroupId == groupId && (textFilter == null || x.Name.Contains(textFilter)))
            .OrderByDescending(x => x.ReadOnly)
            .ThenBy(x => x.Id)
            .ToList();
    }

    public List<GroupRoleLightDTO> GetLightByGroup(int groupId)
    {
        return GetMappedList<GroupRoleLightDTO>(x => x.GroupId == groupId)
            .OrderBy(x => x.Name)
            .ToList();
    }

    private int CreateAdminRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Administrator",
            ReadOnly = true,
            IsDefault = false,
            GroupEdit = true,
            GroupClose = true,
            ReadNoteList = true,
            ReadNote = true,
            CreateNote = true,
            DeleteNote = true,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = true,
            ReadRoleList = true,
            EditRoleList = true,
            ReadGroupActionLog = true,
            ReadNoteActionLog = true,
            ReadTagList = true,
            EditTagList = true
        };

        return Create(role);
    }

    private int CreateModeratorRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Moderator",
            ReadOnly = true,
            IsDefault = false,
            GroupEdit = true,
            GroupClose = false,
            ReadNoteList = true,
            ReadNote = true,
            CreateNote = true,
            DeleteNote = true,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = false,
            ReadRoleList = true,
            EditRoleList = false,
            ReadGroupActionLog = true,
            ReadNoteActionLog = true,
            ReadTagList = true,
            EditTagList = true
        };

        return Create(role);
    }

    private int CreateDefaultRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Default",
            ReadOnly = true,
            IsDefault = true,
            GroupEdit = false,
            GroupClose = false,
            ReadNoteList = true,
            ReadNote = true,
            CreateNote = true,
            DeleteNote = false,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = false,
            ReadRoleList = false,
            EditRoleList = false,
            ReadGroupActionLog = false,
            ReadNoteActionLog = false,
            ReadTagList = true,
            EditTagList = false
        };

        return Create(role);
    }

    public override int Create(GroupRole entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        int id = base.Create(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.RoleCreate);

        return id;
    }

    public override void Update(GroupRole entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Update(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.RoleEdit);
    }

    public override void Delete(GroupRole entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Delete(entity, doPersist);

        groupActionLogService.AddActionLog(entity.GroupId, userId, GroupActionLogType.RoleRemove);
    }

    public bool Exists(int groupId, string name) => GetList(x => x.GroupId == groupId && x.Name == name).Any();

    public class RoleCreationResultItem
    {
        public int Id { get; set; }
        public bool IsAdministration { get; set; }
    }

}

using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupService : MapperRepository<Group, int, string>, IGroupService
{
    private readonly IGroupRoleService groupRoleService;
    private readonly IGroupMemberService groupMemberService;
    private readonly IGroupActionLogService groupActionLogService;
    private readonly IUserService userService;

    public GroupService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper, IGroupRoleService groupRoleService, IGroupMemberService groupMemberService, IGroupActionLogService groupActionLogService, IUserService userService) : base(context, loggerService, utilsService, mapper, "Group")
    {
        this.groupRoleService = groupRoleService;
        this.groupMemberService = groupMemberService;
        this.groupActionLogService = groupActionLogService;
        this.userService = userService;
    }

    public List<GroupListDTO> GetUserList(bool hideClosed = false)
    {
        string userId = Utils.GetRequiredCurrentUserId();

        return GetMappedList<GroupListDTO>(x => (x.OwnerId == userId || x.Members.Any(m => m.UserId == userId)) && (!hideClosed || !x.IsClosed))
            .OrderBy(x => x.IsClosed)
            .ThenByDescending(x => x.Creation)
            .ToList();
    }

    public override int CreateFromModel<TModel>(TModel model, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var id = base.CreateFromModel(model, doPersist);

        var result = groupRoleService.CreateDefaultRoles(id);
        var admin = ObjectHelper.OrElseThrow(result.FirstOrDefault(x => x.IsAdministration), () => new ArgumentException("Admin role not found"));

        // Add current user as administrator
        var member = new GroupMember
        {
            GroupId = id,
            UserId = userId,
            RoleId = admin.Id,
            Creation = DateTime.Now
        };
        groupMemberService.Create(member);

        return id;
    }

    public override int Create(Group entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var id = base.Create(entity, doPersist);

        groupActionLogService.AddActionLog(id, userId, GroupActionLogType.Create);

        return id;
    }

    public override void Update(Group entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Update(entity, doPersist);

        groupActionLogService.AddActionLog(entity.Id, userId, GroupActionLogType.DataEdit);
    }

    public async Task Close(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        ExceptionHelper.Check(!group.IsClosed && await HasFullAccess(group, userId), "Not have permission to Close this group.", "Server.Message.MissingCloseRight");

        group.IsClosed = true;
        Update(group);

        groupActionLogService.AddActionLog(group.Id, userId, GroupActionLogType.Close);
    }

    public async Task<GroupRightsDTO> GetRights(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);
        var hasFullAccess = await HasFullAccess(group, userId);

        var role = GetGroupRole(group, userId);

        return new GroupRightsDTO
        {
            CanClose = hasFullAccess && !group.IsClosed,
            CanOpen = hasFullAccess && group.IsClosed,
            CanRemove = hasFullAccess,
            CanEdit = !group.IsClosed && (hasFullAccess || (role?.GroupEdit ?? false))
        };
    }

    public async Task Open(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        ExceptionHelper.Check(group.IsClosed && await HasFullAccess(group, userId), "Not have permission to Open this group.", "Server.Message.MissingOpenRight");

        group.IsClosed = false;
        Update(group);

        groupActionLogService.AddActionLog(group.Id, userId, GroupActionLogType.Open);
    }

    public async Task Remove(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        ExceptionHelper.Check(await HasFullAccess(group, userId), "Not have permission to Remove this group.", "Server.Message.MissingRemoveRight");

        Delete(group);
    }
    public async Task<GroupTagRightsDTO> GetTagRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        if (await HasFullAccess(group, userId))
        {
            return new GroupTagRightsDTO
            {
                CanCreate = !group.IsClosed,
                CanEdit = !group.IsClosed,
                CanRemove = !group.IsClosed,
                CanView = true
            };
        }

        var role = GetGroupRole(group, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupTagRightsDTO();
        }

        return new GroupTagRightsDTO
        {
            CanCreate = !group.IsClosed && role.EditTagList,
            CanEdit = !group.IsClosed && role.EditTagList,
            CanRemove = !group.IsClosed && role.EditTagList,
            CanView = role.ReadTagList || role.EditTagList
        };
    }

    public async Task<GroupMemberRightsDTO> GetMemberRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        if (await HasFullAccess(group, userId))
        {
            return new GroupMemberRightsDTO
            {
                CanAdd = !group.IsClosed,
                CanEdit = !group.IsClosed,
                CanView = true
            };
        }

        var role = GetGroupRole(group, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupMemberRightsDTO();
        }

        return new GroupMemberRightsDTO
        {
            CanAdd = !group.IsClosed && role.EditMemberList,
            CanEdit = !group.IsClosed && role.EditMemberList,
            CanView = role.ReadMemberList || role.EditMemberList
        };
    }

    public async Task<GroupRoleRightsDTO> GetRoleRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        if (await HasFullAccess(group, userId))
        {
            return new GroupRoleRightsDTO
            {
                CanCreate = !group.IsClosed,
                CanEdit = !group.IsClosed,
                CanView = true,
            };
        }

        var role = GetGroupRole(group, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupRoleRightsDTO();
        }

        return new GroupRoleRightsDTO
        {
            CanCreate = !group.IsClosed && role.EditRoleList,
            CanEdit = !group.IsClosed && role.EditRoleList,
            CanView = role.ReadRoleList || role.EditRoleList
        };
    }

    public GroupRole? GetUserRole(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        return Get(id).Members.FirstOrDefault(x => x.UserId == userId)?.Role;
    }

    public bool IsCurrentOwner(int id)
    {
        var group = Get(id);

        var userId = Utils.GetCurrentUserId();

        if (ObjectHelper.IsNull(userId))
        {
            return false;
        }

        return group.OwnerId == userId;
    }

    public async Task<GroupPageRightsDTO> GetPageRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        if (await HasFullAccess(group, userId))
        {
            return new GroupPageRightsDTO(true);
        }

        var role = GetGroupRole(group, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupPageRightsDTO();
        }

        return new GroupPageRightsDTO
        {
            DataPageEnabled = true,
            LogPageEnabled = role.ReadGroupActionLog,
            MemberPageEnabled = role.ReadMemberList || role.EditMemberList,
            RolePageEnabled = role.ReadRoleList || role.EditRoleList,
            NotePageEnabled = role.ReadNoteList || role.ReadNote || role.EditNote || role.DeleteNote,
            TagPageEnabled = role.ReadTagList || role.EditTagList
        };
    }

    public GroupRole? GetGroupRole(Group group, string userId)
    {
        var roleId = group.Members.FirstOrDefault(x => x.UserId == userId)?.RoleId;

        if (ObjectHelper.IsNull(roleId))
        {
            return null;
        }

        return groupRoleService.Get((int)roleId);
    }

    public async Task<bool> HasFullAccess(Group group, string userId)
    {
        var admin = await userService.IsAdministrator();

        return admin || group.OwnerId == userId;
    }

    public async Task<GroupNoteRightsDTO> GetNoteRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        if (await HasFullAccess(group, userId))
        {
            return new GroupNoteRightsDTO
            {
                CanCreate = !group.IsClosed,
                CanView = true,
                CanOpen = true,
            };
        }

        var role = GetGroupRole(group, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupNoteRightsDTO();
        }

        return new GroupNoteRightsDTO
        {
            CanCreate = !group.IsClosed && role.EditNote,
            CanView = role.ReadNoteList || role.ReadNote || role.EditNote || role.DeleteNote,
            CanOpen = role.ReadNote || role.EditNote || role.DeleteNote,
        };
    }
}

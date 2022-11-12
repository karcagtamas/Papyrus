using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Logic.Services.Security;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;
using Papyrus.Shared.Enums.Groups;
using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Services.Groups;

public class GroupService : MapperRepository<Group, int, string>, IGroupService
{
    private readonly IGroupRoleService groupRoleService;
    private readonly IGroupMemberService groupMemberService;
    private readonly IGroupActionLogService groupActionLogService;
    private readonly IUserService userService;
    private readonly IFolderService folderService;

    public GroupService(
        PapyrusContext context,
        ILoggerService loggerService,
        IUtilsService<string> utilsService,
        IMapper mapper,
        IGroupRoleService groupRoleService,
        IGroupMemberService groupMemberService,
        IGroupActionLogService groupActionLogService,
        IUserService userService,
        IFolderService folderService
        ) : base(context, loggerService, utilsService, mapper, "Group")
    {
        this.folderService = folderService;
        this.groupRoleService = groupRoleService;
        this.groupMemberService = groupMemberService;
        this.groupActionLogService = groupActionLogService;
        this.userService = userService;
    }

    public List<GroupListDTO> GetUserList(bool hideClosed = false)
    {
        string userId = Utils.GetRequiredCurrentUserId();

        return MapFromQuery<GroupListDTO>(
            GetListAsQuery(x => (x.OwnerId == userId || x.Members.Any(m => m.UserId == userId)) && (!hideClosed || !x.IsClosed))
                .Include(x => x.Owner)
                .OrderBy(x => x.IsClosed)
                .ThenByDescending(x => x.Creation))
            .ToList();
    }

    public override int CreateFromModel<TModel>(TModel model, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var id = base.CreateFromModel(model, doPersist);

        var result = groupRoleService.CreateDefaultRoles(id);
        var admin = ObjectHelper.OrElseThrow(result.FirstOrDefault(x => x.IsAdministrator), () => new ArgumentException("Admin role not found"));

        // Add current user as administrator
        var member = new GroupMember
        {
            GroupId = id,
            UserId = userId,
            RoleId = admin.Id,
            Creation = DateTime.Now
        };
        groupMemberService.Create(member);

        // Add Note root folder
        folderService.CreateRootFolder(null, id);

        return id;
    }

    public override int Create(Group entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var id = base.Create(entity, doPersist);

        groupActionLogService.AddActionLog(id, userId, GroupActionLogType.Create, doPersist);

        return id;
    }

    public override void Update(Group entity, bool doPersist = true)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        base.Update(entity, doPersist);

        groupActionLogService.AddActionLog(entity.Id, userId, GroupActionLogType.DataEdit, doPersist);
    }

    public async Task Close(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        ExceptionHelper.Check(!group.IsClosed && await HasFullAccess(group, userId), "Not have permission to Close this group.", "Server.Message.MissingCloseRight");

        group.IsClosed = true;
        Update(group);

        groupActionLogService.AddActionLog(group.Id, userId, GroupActionLogType.Close, true);
    }

    public async Task<GroupRightsDTO> GetRights(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);
        var hasFullAccess = await HasFullAccess(group, userId);

        var role = GetGroupRole(id, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupRightsDTO(false);
        }

        return new GroupRightsDTO
        {
            CanClose = AuthorizationService.CheckUserGroupRight(GroupRight.Close, group, role, hasFullAccess),
            CanOpen = AuthorizationService.CheckUserGroupRight(GroupRight.Open, group, role, hasFullAccess),
            CanRemove = AuthorizationService.CheckUserGroupRight(GroupRight.Remove, group, role, hasFullAccess),
            CanEdit = AuthorizationService.CheckUserGroupRight(GroupRight.Edit, group, role, hasFullAccess),
            CanOpenNote = AuthorizationService.CheckUserGroupRight(GroupRight.ReadNote, group, role, hasFullAccess)
        };
    }

    public async Task Open(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        ExceptionHelper.Check(group.IsClosed && await HasFullAccess(group, userId), "Not have permission to Open this group.", "Server.Message.MissingOpenRight");

        group.IsClosed = false;
        Update(group);

        groupActionLogService.AddActionLog(group.Id, userId, GroupActionLogType.Open, true);
    }

    public async Task Remove(int id)
    {
        string userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);

        ExceptionHelper.Check(await HasFullAccess(group, userId), "Not have permission to Remove this group.", "Server.Message.MissingRemoveRight");

        Delete(group);

        groupActionLogService.AddActionLog(group.Id, userId, GroupActionLogType.Remove, true);
    }
    public async Task<GroupTagRightsDTO> GetTagRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);
        var hasFullAccess = await HasFullAccess(group, userId);

        var role = GetGroupRole(id, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupTagRightsDTO(false);
        }

        var manageRight = AuthorizationService.CheckUserGroupRight(GroupRight.ManageTag, group, role, hasFullAccess);

        return new GroupTagRightsDTO
        {
            CanCreate = manageRight,
            CanEdit = manageRight,
            CanRemove = manageRight,
            CanView = AuthorizationService.CheckUserGroupRight(GroupRight.ReadTags, group, role, hasFullAccess)
        };
    }

    public async Task<GroupMemberRightsDTO> GetMemberRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);
        var hasFullAccess = await HasFullAccess(group, userId);

        var role = GetGroupRole(id, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupMemberRightsDTO(false);
        }

        var editRight = AuthorizationService.CheckUserGroupRight(GroupRight.EditMembers, group, role, hasFullAccess);

        return new GroupMemberRightsDTO
        {
            CanAdd = editRight,
            CanEdit = editRight,
            CanView = AuthorizationService.CheckUserGroupRight(GroupRight.ReadMembers, group, role, hasFullAccess)
        };
    }

    public async Task<GroupRoleRightsDTO> GetRoleRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);
        var hasFullAccess = await HasFullAccess(group, userId);

        if (await HasFullAccess(group, userId))
        {
            return new GroupRoleRightsDTO
            {
                CanCreate = !group.IsClosed,
                CanEdit = !group.IsClosed,
                CanView = true,
            };
        }

        var role = GetGroupRole(id, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupRoleRightsDTO();
        }

        var editRight = AuthorizationService.CheckUserGroupRight(GroupRight.EditRoles, group, role, hasFullAccess);

        return new GroupRoleRightsDTO
        {
            CanCreate = editRight,
            CanEdit = editRight,
            CanView = AuthorizationService.CheckUserGroupRight(GroupRight.ReadRoles, group, role, hasFullAccess)
        };
    }

    public bool IsCurrentOwner(int id) => IsUserOwner(id, Utils.GetCurrentUser<User>());

    public bool IsUserOwner(int id, User user)
    {
        var group = Get(id);

        if (ObjectHelper.IsNull(user))
        {
            return false;
        }

        return group.OwnerId == user.Id;
    }

    public async Task<GroupPageRightsDTO> GetPageRights(int id)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        var group = Get(id);
        var hasFullAccess = await HasFullAccess(group, userId);

        var role = GetGroupRole(id, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupPageRightsDTO(false);
        }

        return new GroupPageRightsDTO
        {
            DataPageEnabled = true,
            LogPageEnabled = AuthorizationService.CheckUserGroupRight(GroupRight.ReadLogs, group, role, hasFullAccess),
            MemberPageEnabled = AuthorizationService.CheckUserGroupRight(GroupRight.ReadMembers, group, role, hasFullAccess),
            RolePageEnabled = AuthorizationService.CheckUserGroupRight(GroupRight.ReadRoles, group, role, hasFullAccess),
            NotePageEnabled = AuthorizationService.CheckUserGroupRight(GroupRight.ReadNotes, group, role, hasFullAccess),
            TagPageEnabled = AuthorizationService.CheckUserGroupRight(GroupRight.ReadTags, group, role, hasFullAccess)
        };
    }

    public GroupRole? GetGroupRole(int groupId, string userId)
    {
        return Context.Set<GroupMember>().AsQueryable()
            .Where(x => x.GroupId == groupId && x.UserId == userId)
            .Include(x => x.Role)
            .FirstOrDefault()?
            .Role;
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
        var hasFullAccess = await HasFullAccess(group, userId);

        var role = GetGroupRole(id, userId);

        if (ObjectHelper.IsNull(role))
        {
            return new GroupNoteRightsDTO(false);
        }

        var manageAccess = AuthorizationService.CheckUserGroupRight(GroupRight.ManageNote, group, role, hasFullAccess);

        return new GroupNoteRightsDTO
        {
            CanCreateNote = manageAccess,
            CanViewNote = AuthorizationService.CheckUserGroupRight(GroupRight.ReadNotes, group, role, hasFullAccess),
            CanOpenNote = AuthorizationService.CheckUserGroupRight(GroupRight.ReadNote, group, role, hasFullAccess),
            CanCreateFolder = manageAccess,
            CanManageFolder = manageAccess,
            CanEditNote = manageAccess,
            CanDeleteNote = manageAccess,
        };
    }

    public List<GroupNoteListDTO> GetRecentEdits(int id)
    {
        var group = Get(id);

        return Mapper.Map<List<GroupNoteListDTO>>(
            group.Notes
                .Where(x => x.ContentLastEdit != null)
                .OrderByDescending(x => x.ContentLastEdit)
                .Take(10)
                .ToList()
        );
    }
}

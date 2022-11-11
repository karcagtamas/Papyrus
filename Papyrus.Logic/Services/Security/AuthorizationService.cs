using KarcagS.Shared.Helpers;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Logic.Services.Security.Interfaces;
using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Services.Security;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserService userService;
    private readonly IApplicationService applicationService;
    private readonly IGroupService groupService;

    public AuthorizationService(IUserService userService, IApplicationService applicationService, IGroupService groupService)
    {
        this.userService = userService;
        this.applicationService = applicationService;
        this.groupService = groupService;
    }

    public Task<bool> UserHasApplicationAccessRight(string userId, string appId)
    {
        return WithUser(userId, (user) =>
        {
            var app = applicationService.GetOptional(appId);

            if (ObjectHelper.IsNull(app))
            {
                return false;
            }

            return app.UserId == user.Id;
        });
    }

    public Task<bool> UserHasGroupRight(string userId, int groupId, GroupRight right)
    {
        return WithGroup(userId, groupId, (user, group) =>
        {
            var role = group.Members.FirstOrDefault(x => x.UserId == user.Id)?.Role;

            if (ObjectHelper.IsNull(role))
            {
                return false;
            }

            return CheckUserGroupRight(right, group, role);
        });
    }

    public static bool CheckUserGroupRight(GroupRight right, Group group, GroupRole role, bool fullAccess = false)
    {
        return right switch
        {
            GroupRight.Read => true,
            GroupRight.Edit => (fullAccess || role.GroupEdit) && !group.IsClosed,
            GroupRight.Close => fullAccess && !group.IsClosed,
            GroupRight.Open => fullAccess && group.IsClosed,
            GroupRight.Remove => fullAccess,
            GroupRight.ReadLogs => fullAccess || role.ReadGroupActionLog,
            GroupRight.ReadRoles => fullAccess || role.ReadRoleList || role.EditRoleList,
            GroupRight.EditRoles => (fullAccess || role.EditRoleList) && !group.IsClosed,
            GroupRight.ReadMembers => fullAccess || role.ReadMemberList || role.EditMemberList,
            GroupRight.EditMembers => (fullAccess || role.EditMemberList) && !group.IsClosed,
            GroupRight.ReadNotes => fullAccess || role.ReadNoteList || role.ReadNote || role.EditNote || role.DeleteNote,
            GroupRight.CreateNote => (fullAccess || role.EditNote || role.DeleteNote) && !group.IsClosed,
            GroupRight.CreateFolder => (fullAccess || role.EditNote || role.DeleteNote) && !group.IsClosed,
            GroupRight.ReadTags => fullAccess || role.ReadTagList || role.EditTagList,
            GroupRight.CreateTag => (fullAccess || role.EditTagList) && !group.IsClosed,
            _ => false,
        }; ;
    }

    private async Task<bool> WithUser(string userId, Func<User, bool> func)
    {
        var user = userService.Get(userId);

        var isAdmin = await userService.IsUserAdministrator(user);

        return isAdmin || func(user);
    }

    private Task<bool> WithGroup(string userId, int groupId, Func<User, Group, bool> func)
    {
        return WithUser(userId, (user) =>
        {
            var group = groupService.GetOptional(groupId);

            if (ObjectHelper.IsNull(group))
            {
                return false;
            }

            return groupService.IsUserOwner(group.Id, user) || func(user, group);
        });
    }
}

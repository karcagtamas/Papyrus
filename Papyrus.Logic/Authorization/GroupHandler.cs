using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Logic.Authorization;

public class GroupHandler : AuthorizationHandler<GroupRequirement, int>
{
    private readonly IGroupService groupService;
    private readonly IUserService userService;
    private static readonly List<GroupAuthorization> checkers = new();

    public GroupHandler(IGroupService groupService, IUserService userService)
    {
        this.groupService = groupService;
        this.userService = userService;
        RegisterCheckers();
    }

    private static void RegisterCheckers()
    {
        checkers.Clear();
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.ReadGroupRequirement,
            Checker = (input) => true
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.EditGroupRequirement,
            Checker = (input) => input.GroupRole.GroupEdit
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.CloseOpenGroupRequirement,
            Checker = (input) => input.GroupRights.CanClose || input.GroupRights.CanOpen
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.RemoveGroupRequirement,
            Checker = (input) => input.GroupRights.CanRemove
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.ReadGroupLogsRequirement,
            Checker = (input) => input.GroupRole.ReadGroupActionLog
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.ReadGroupRolesRequirement,
            Checker = (input) => input.GroupRole.ReadRoleList || input.GroupRole.EditRoleList
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.EditGroupRolesRequirement,
            Checker = (input) => input.GroupRole.EditRoleList
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.ReadGroupMembersRequirement,
            Checker = (input) => input.GroupRole.ReadMemberList || input.GroupRole.EditMemberList
        });
        checkers.Add(new GroupAuthorization
        {
            Requirement = GroupOperations.EditGroupMembersRequirement,
            Checker = (input) => input.GroupRole.EditMemberList
        });
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRequirement requirement, int resource)
    {
        var admin = await userService.IsAdministrator();

        if (admin || groupService.IsCurrentOwner(resource))
        {
            context.Succeed(requirement);
            return;
        }

        var rights = groupService.GetUserRole(resource);
        var groupRights = await groupService.GetRights(resource);

        if (ObjectHelper.IsNull(rights))
        {
            return;
        }

        var check = checkers.FirstOrDefault(x => x.Requirement == requirement);

        ObjectHelper.WhenNotNull(check, c =>
        {
            if (c.Checker(new GroupAuthorizationInput { GroupRole = rights, GroupRights = groupRights }))
            {
                context.Succeed(c.Requirement);
            }
        });
    }

    public class GroupAuthorization
    {
        public GroupRequirement Requirement { get; set; } = default!;
        public Func<GroupAuthorizationInput, bool> Checker { get; set; } = (input) => true;
    }

    public class GroupAuthorizationInput
    {
        public GroupRole GroupRole { get; set; } = default!;
        public GroupRightsDTO GroupRights { get; set; } = default!;
    }
}

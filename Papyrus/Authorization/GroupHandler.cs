using Microsoft.AspNetCore.Authorization;
using Papyrus.Logic.Services.Groups.Interfaces;

namespace Papyrus.Authorization;

public class GroupHandler : AuthorizationHandler<GroupRequirement, int>
{
    private readonly IGroupService groupService;

    public GroupHandler(IGroupService groupService)
    {
        this.groupService = groupService;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRequirement requirement, int resource)
    {
        var rights = groupService.GetUserRole(resource);
        var groupRights = groupService.GetRights(resource);

        if (ObjectHelper.IsNull(rights))
        {
            return Task.CompletedTask;
        }

        if (requirement == GroupOperations.ReadGroupRequirement)
        {
            context.Succeed(requirement);
        }
        else if (requirement == GroupOperations.EditGroupRequirement)
        {
            if (rights.GroupEdit)
            {
                context.Succeed(requirement);
            }
        }
        else if (requirement == GroupOperations.CloseOpenGroupRequirement)
        {
            if (groupRights.CanClose || groupRights.CanOpen)
            {
                context.Succeed(requirement);
            }
        }
        else if (requirement == GroupOperations.RemoveGroupRequirement)
        {
            if (groupRights.CanRemove)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}

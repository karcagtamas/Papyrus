using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Papyrus.Authorization;

public class GroupRequirement : OperationAuthorizationRequirement
{
}

public static class GroupOperations
{
    public static GroupRequirement ReadGroupRequirement = new() { Name = "ReadGroup" };
    public static GroupRequirement EditGroupRequirement = new() { Name = "EditGroup" };
    public static GroupRequirement CloseOpenGroupRequirement = new() { Name = "CloseOpenGroup" };
    public static GroupRequirement RemoveGroupRequirement = new() { Name = "RemoveGroup" };
}

public static class GroupPolicies
{
    public static AuthorizationPolicy ReadGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.ReadGroupRequirement)
           .Build();
    public static AuthorizationPolicy EditGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.EditGroupRequirement)
           .Build();
    public static AuthorizationPolicy CloseOpenGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.CloseOpenGroupRequirement)
           .Build();
    public static AuthorizationPolicy RemoveGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.RemoveGroupRequirement)
           .Build();
}
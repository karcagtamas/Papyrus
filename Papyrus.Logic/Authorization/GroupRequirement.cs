using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Papyrus.Logic.Authorization;

public class GroupRequirement : OperationAuthorizationRequirement
{
}

public static class GroupOperations
{
    public static readonly GroupRequirement ReadGroupRequirement = new() { Name = "ReadGroup" };
    public static readonly GroupRequirement EditGroupRequirement = new() { Name = "EditGroup" };
    public static readonly GroupRequirement CloseOpenGroupRequirement = new() { Name = "CloseOpenGroup" };
    public static readonly GroupRequirement RemoveGroupRequirement = new() { Name = "RemoveGroup" };
    public static readonly GroupRequirement ReadGroupLogsRequirement = new() { Name = "ReadGroupLogs" };
    public static readonly GroupRequirement ReadGroupRolesRequirement = new() { Name = "ReadGroupRoles" };
    public static readonly GroupRequirement EditGroupRolesRequirement = new() { Name = "EditGroupRoles" };
    public static readonly GroupRequirement ReadGroupMembersRequirement = new() { Name = "ReadGroupMembers" };
    public static readonly GroupRequirement EditGroupMembersRequirement = new() { Name = "EditGroupMembers" };
}

public static class GroupPolicies
{
    public static readonly AuthorizationPolicy ReadGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.ReadGroupRequirement)
           .Build();
    public static readonly AuthorizationPolicy EditGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.EditGroupRequirement)
           .Build();
    public static readonly AuthorizationPolicy CloseOpenGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.CloseOpenGroupRequirement)
           .Build();
    public static readonly AuthorizationPolicy RemoveGroup = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.RemoveGroupRequirement)
           .Build();
    public static readonly AuthorizationPolicy ReadGroupLogs = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.ReadGroupLogsRequirement)
           .Build();
    public static readonly AuthorizationPolicy ReadGroupRoles = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.ReadGroupRolesRequirement)
           .Build();
    public static readonly AuthorizationPolicy EditGroupRoles = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.EditGroupRolesRequirement)
           .Build();
    public static readonly AuthorizationPolicy ReadGroupMembers = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.ReadGroupMembersRequirement)
           .Build();
    public static readonly AuthorizationPolicy EditGroupMembers = new AuthorizationPolicyBuilder()
           .AddRequirements(GroupOperations.EditGroupMembersRequirement)
           .Build();
}
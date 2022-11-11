using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Authorization;

public class GroupRequirement : OperationAuthorizationRequirement
{
    public GroupRight Right { get; set; }
}

public static class GroupOperations
{
    public static readonly GroupRequirement ReadGroupRequirement = new() { Name = "ReadGroup", Right = GroupRight.Read };
    public static readonly GroupRequirement EditGroupRequirement = new() { Name = "EditGroup", Right = GroupRight.Edit };
    public static readonly GroupRequirement CloseGroupRequirement = new() { Name = "CloseGroup", Right = GroupRight.Close };
    public static readonly GroupRequirement OpenGroupRequirement = new() { Name = "OpenGroup", Right = GroupRight.Open };
    public static readonly GroupRequirement RemoveGroupRequirement = new() { Name = "RemoveGroup", Right = GroupRight.Remove };
    public static readonly GroupRequirement ReadGroupLogsRequirement = new() { Name = "ReadGroupLogs", Right = GroupRight.ReadLogs };
    public static readonly GroupRequirement ReadGroupRolesRequirement = new() { Name = "ReadGroupRoles", Right = GroupRight.ReadRoles };
    public static readonly GroupRequirement EditGroupRolesRequirement = new() { Name = "EditGroupRoles", Right = GroupRight.EditRoles };
    public static readonly GroupRequirement ReadGroupMembersRequirement = new() { Name = "ReadGroupMembers", Right = GroupRight.ReadMembers };
    public static readonly GroupRequirement EditGroupMembersRequirement = new() { Name = "EditGroupMembers", Right = GroupRight.EditMembers };
    public static readonly GroupRequirement ReadNotesRequirement = new() { Name = "ReadNotes", Right = GroupRight.ReadNotes };
    public static readonly GroupRequirement CreateNoteRequirement = new() { Name = "CreateNote", Right = GroupRight.CreateNote };
    public static readonly GroupRequirement CreateFolderRequirement = new() { Name = "CreateFolder", Right = GroupRight.CreateFolder };
    public static readonly GroupRequirement ReadTagsRequirement = new() { Name = "ReadTag", Right = GroupRight.ReadTags };
    public static readonly GroupRequirement CreateTagRequirement = new() { Name = "CreateTag", Right = GroupRight.CreateTag };
}

public static class GroupPolicies
{
    public static readonly AuthorizationPolicy ReadGroup = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.ReadGroupRequirement)
            .Build();
    public static readonly AuthorizationPolicy EditGroup = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.EditGroupRequirement)
            .Build();
    public static readonly AuthorizationPolicy CloseGroup = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.CloseGroupRequirement)
            .Build();
    public static readonly AuthorizationPolicy OpenGroup = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.OpenGroupRequirement)
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
    public static readonly AuthorizationPolicy ReadNotes = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.ReadNotesRequirement)
            .Build();
    public static readonly AuthorizationPolicy CreateNote = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.CreateNoteRequirement)
            .Build();
    public static readonly AuthorizationPolicy CreateFolder = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.CreateFolderRequirement)
            .Build();
    public static readonly AuthorizationPolicy ReadTags = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.ReadTagsRequirement)
            .Build();
    public static readonly AuthorizationPolicy CreateTag = new AuthorizationPolicyBuilder()
            .AddRequirements(GroupOperations.CreateTagRequirement)
            .Build();
}

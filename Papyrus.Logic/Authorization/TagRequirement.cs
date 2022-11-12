using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Authorization;

public class TagRequirement : OperationAuthorizationRequirement
{
    public TagRight Right { get; set; }
}

public static class TagOperations
{
    public static readonly TagRequirement ReadTagRequirement = new() { Name = "ReadTag", Right = TagRight.Read };
    public static readonly TagRequirement EditTagRequirement = new() { Name = "EditTag", Right = TagRight.Edit };
}

public static class TagPolicies
{
    public static readonly AuthorizationPolicy ReadTag = new AuthorizationPolicyBuilder()
          .AddRequirements(TagOperations.ReadTagRequirement)
          .Build();
    public static readonly AuthorizationPolicy EditTag = new AuthorizationPolicyBuilder()
           .AddRequirements(TagOperations.EditTagRequirement)
           .Build();
}

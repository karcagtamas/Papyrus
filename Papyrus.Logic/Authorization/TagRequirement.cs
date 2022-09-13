using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Papyrus.Logic.Authorization;

public class TagRequirement : OperationAuthorizationRequirement
{
}

public static class TagOperations
{
    public static readonly TagRequirement ReadTagRequirement = new() { Name = "ReadTag" };
    public static readonly TagRequirement EditTagRequirement = new() { Name = "EditTag" };
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

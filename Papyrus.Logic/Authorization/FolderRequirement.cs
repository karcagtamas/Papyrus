using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Papyrus.Logic.Authorization;

public class FolderRequirement : OperationAuthorizationRequirement
{
}

public static class FolderOperations
{
    public static readonly FolderRequirement ReadFolderRequirement = new() { Name = "ReadFolder" };
    public static readonly FolderRequirement ManageFolderRequirement = new() { Name = "ManageFolder" };
}

public static class FolderPolicies
{

    public static readonly AuthorizationPolicy ReadFolder = new AuthorizationPolicyBuilder()
            .AddRequirements(FolderOperations.ReadFolderRequirement)
            .Build();
    public static readonly AuthorizationPolicy ManagerFolder = new AuthorizationPolicyBuilder()
            .AddRequirements(FolderOperations.ManageFolderRequirement)
            .Build();
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Authorization;

public class FolderRequirement : OperationAuthorizationRequirement
{
    public FolderRight Right { get; set; }
}

public static class FolderOperations
{
    public static readonly FolderRequirement ReadFolderRequirement = new() { Name = "ReadFolder", Right = FolderRight.Read };
    public static readonly FolderRequirement ManageFolderRequirement = new() { Name = "ManageFolder", Right = FolderRight.Manage };
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

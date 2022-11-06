using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Papyrus.Logic.Authorization;

public class ApplicationRequirement : OperationAuthorizationRequirement
{
}

public static class ApplicationOperations
{
    public static readonly ApplicationRequirement AccessApplicationRequirement = new() { Name = "AccessApplication" };
}

public static class ApplicationPolicies
{

    public static readonly AuthorizationPolicy AccessApplication = new AuthorizationPolicyBuilder()
            .AddRequirements(ApplicationOperations.AccessApplicationRequirement)
            .Build();
}

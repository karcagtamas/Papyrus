using KarcagS.Common.Tools.Services;
using Microsoft.AspNetCore.Authorization;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Profile.Interfaces;

namespace Papyrus.Logic.Authorization;

public class ApplicationHandler : AuthorizationHandler<ApplicationRequirement, string>
{
    private readonly IUserService userService;
    private readonly IApplicationService applicationService;
    private readonly IUtilsService<string> utils;
    private readonly ILoggerService logger;

    public ApplicationHandler(IUserService userService, IApplicationService applicationService, IUtilsService<string> utils, ILoggerService logger)
    {
        this.logger = logger;
        this.utils = utils;
        this.applicationService = applicationService;
        this.userService = userService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationRequirement requirement, string resource)
    {
        try
        {
            if (await userService.IsAdministrator())
            {
                context.Succeed(requirement);
                return;
            }

            var app = applicationService.Get(resource);
            var userId = utils.GetRequiredCurrentUserId();

            if (app.UserId == userId)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
            return;
        }
        catch (Exception e)
        {
            context.Fail();
            logger.LogError(e);
        }
    }
}

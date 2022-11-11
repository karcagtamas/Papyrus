using KarcagS.Common.Tools.Services;
using Microsoft.AspNetCore.Authorization;
using Auth = Papyrus.Logic.Services.Security.Interfaces;

namespace Papyrus.Logic.Authorization;

public class NoteHandler : AuthorizationHandler<NoteRequirement, string>
{
    private readonly Auth.IAuthorizationService authorizationService;
    private readonly IUtilsService<string> utils;
    private readonly ILoggerService logger;

    public NoteHandler(Auth.IAuthorizationService authorizationService, IUtilsService<string> utils, ILoggerService logger)
    {
        this.authorizationService = authorizationService;
        this.utils = utils;
        this.logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NoteRequirement requirement, string resource)
    {
        try
        {
            var userId = utils.GetRequiredCurrentUserId();
            if (await authorizationService.UserHasNoteRight(userId, resource, requirement.Right))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
        catch (Exception e)
        {
            context.Fail();
            logger.LogError(e);
        }
    }
}

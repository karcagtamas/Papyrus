using KarcagS.Common.Tools.Services;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities.Groups;
using Auth = Papyrus.Logic.Services.Security.Interfaces;

namespace Papyrus.Logic.Authorization;

public class TagHandler : AuthorizationHandler<TagRequirement, int>
{
    private readonly Auth.IAuthorizationService authorizationService;
    private readonly IUtilsService<string> utils;
    private readonly ILoggerService logger;

    public TagHandler(Auth.IAuthorizationService authorizationService, IUtilsService<string> utils, ILoggerService logger)
    {
        this.authorizationService = authorizationService;
        this.utils = utils;
        this.logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TagRequirement requirement, int resource)
    {
        try
        {
            var userId = utils.GetRequiredCurrentUserId();
            if (await authorizationService.UserHasTagRight(userId, resource, requirement.Right))
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

    public class TagAuthorization
    {
        public TagRequirement Requirement { get; set; } = default!;
        public Func<TagAuthorizationInput, bool> Checker { get; set; } = (input) => true;
    }

    public class TagAuthorizationInput
    {
        public GroupRole GroupRole { get; set; } = default!;
        public bool GroupCase { get; set; } = default!;
        public bool GroupIsClosed { get; set; } = default!;

        public bool GroupEditCheck { get => !GroupCase || !GroupIsClosed; }
    }
}

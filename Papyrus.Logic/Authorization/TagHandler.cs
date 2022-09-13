using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Logic.Authorization;

public class TagHandler : AuthorizationHandler<TagRequirement, int>
{
    private readonly IUserService userService;
    private readonly IUtilsService<string> utils;
    private readonly ITagService tagService;
    private readonly IGroupService groupService;
    private readonly ILoggerService logger;
    private static readonly List<TagAuthorization> checkers = new();

    public TagHandler(IUserService userService, IUtilsService<string> utils, ITagService tagService, IGroupService groupService, ILoggerService logger)
    {
        RegisterCheckers();
        this.userService = userService;
        this.utils = utils;
        this.tagService = tagService;
        this.groupService = groupService;
        this.logger = logger;
    }

    private static void RegisterCheckers()
    {
        checkers.Clear();
        checkers.Add(new TagAuthorization
        {
            Requirement = TagOperations.ReadTagRequirement,
            Checker = (input) => true
        });
        checkers.Add(new TagAuthorization
        {
            Requirement = TagOperations.EditTagRequirement,
            Checker = (input) => input.EditTagList
        });
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TagRequirement requirement, int resource)
    {
        try
        {
            if (await userService.IsAdministrator())
            {
                context.Succeed(requirement);
                return;
            }

            var tag = tagService.Get(resource);
            var userId = utils.GetRequiredCurrentUserId();

            if (ObjectHelper.IsNotNull(tag.UserId) && tag.UserId == userId)
            {
                context.Succeed(requirement);
                return;
            }

            if (ObjectHelper.IsNotNull(tag.GroupId))
            {
                if (groupService.IsCurrentOwner((int)tag.GroupId))
                {
                    context.Succeed(requirement);
                    return;
                }

                var rights = groupService.GetUserRole((int)tag.GroupId);

                if (ObjectHelper.IsNull(rights))
                {
                    context.Fail();
                    return;
                }

                var check = checkers.FirstOrDefault(x => x.Requirement == requirement);

                ObjectHelper.WhenNotNull(check, c =>
                {
                    if (c.Checker(rights))
                    {
                        context.Succeed(c.Requirement);
                    }
                });
            }
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
        public Func<GroupRole, bool> Checker { get; set; } = (input) => true;
    }
}

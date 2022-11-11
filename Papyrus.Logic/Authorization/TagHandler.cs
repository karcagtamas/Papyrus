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
            Checker = (input) => input.GroupRole.EditTagList && input.GroupEditCheck
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

                GroupRole? rights = null;//groupService.GetUserRole((int)note.GroupId);

                if (ObjectHelper.IsNull(rights))
                {
                    context.Fail();
                    return;
                }

                var group = groupService.Get((int)tag.GroupId);
                var check = checkers.FirstOrDefault(x => x.Requirement == requirement);

                ObjectHelper.WhenNotNull(check, c =>
                {
                    if (c.Checker(new TagAuthorizationInput { GroupRole = rights, GroupCase = true, GroupIsClosed = group.IsClosed }))
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

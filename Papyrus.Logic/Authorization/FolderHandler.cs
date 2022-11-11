using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Logic.Authorization;

public class FolderHandler : AuthorizationHandler<FolderRequirement, string>
{
    private readonly IUserService userService;
    private readonly IUtilsService<string> utils;
    private readonly INoteService noteService;
    private readonly IGroupService groupService;
    private readonly IFolderService folderService;
    private readonly ILoggerService logger;
    private static readonly List<FolderAuthorization> checkers = new();

    public FolderHandler(IUserService userService, IUtilsService<string> utils, INoteService noteService, IGroupService groupService, IFolderService folderService, ILoggerService logger)
    {
        RegisterCheckers();
        this.userService = userService;
        this.utils = utils;
        this.noteService = noteService;
        this.groupService = groupService;
        this.folderService = folderService;
        this.logger = logger;
    }

    private static void RegisterCheckers()
    {
        checkers.Clear();
        checkers.Add(new FolderAuthorization
        {
            Requirement = FolderOperations.ReadFolderRequirement,
            Checker = (rights, folder) => rights.ReadNoteList || rights.ReadNote || rights.EditNote || rights.DeleteNote
        });
        checkers.Add(new FolderAuthorization
        {
            Requirement = FolderOperations.ManageFolderRequirement,
            Checker = (rights, folder) => rights.EditNote || rights.DeleteNote
        });
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FolderRequirement requirement, string resource)
    {
        try
        {
            if (await userService.IsAdministrator())
            {
                context.Succeed(requirement);
                return;
            }

            var folder = folderService.Get(resource);
            var userId = utils.GetRequiredCurrentUserId();

            if (ObjectHelper.IsNotNull(folder.UserId) && folder.UserId == userId)
            {
                context.Succeed(requirement);
                return;
            }

            if (ObjectHelper.IsNotNull(folder.GroupId))
            {
                if (groupService.IsCurrentOwner((int)folder.GroupId))
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

                var check = checkers.FirstOrDefault(x => x.Requirement == requirement);

                ObjectHelper.WhenNotNull(check, c =>
                {
                    if (c.Checker(rights, folder))
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

    public class FolderAuthorization
    {
        public FolderRequirement Requirement { get; set; } = default!;
        public Func<GroupRole, Folder, bool> Checker { get; set; } = (role, folder) => true;
    }
}

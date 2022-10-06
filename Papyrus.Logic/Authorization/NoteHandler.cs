using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Logic.Authorization;

public class NoteHandler : AuthorizationHandler<NoteRequirement, string>
{
    private readonly IUserService userService;
    private readonly IUtilsService<string> utils;
    private readonly INoteService noteService;
    private readonly IGroupService groupService;
    private readonly ILoggerService logger;
    private static readonly List<NoteAuthorization> checkers = new();

    public NoteHandler(IUserService userService, IUtilsService<string> utils, INoteService noteService, IGroupService groupService, ILoggerService logger)
    {
        RegisterCheckers();
        this.userService = userService;
        this.utils = utils;
        this.noteService = noteService;
        this.groupService = groupService;
        this.logger = logger;
    }

    private static void RegisterCheckers()
    {
        checkers.Clear();
        checkers.Add(new NoteAuthorization
        {
            Requirement = NoteOperations.ReadNoteRequirement,
            Checker = (rights, note) => note.Public || rights.ReadNote || rights.EditNote || rights.DeleteNote
        });
        checkers.Add(new NoteAuthorization
        {
            Requirement = NoteOperations.EditNoteRequirement,
            Checker = (rights, note) => rights.EditNote || rights.DeleteNote
        });
        checkers.Add(new NoteAuthorization
        {
            Requirement = NoteOperations.DeleteNoteRequirement,
            Checker = (rights, note) => rights.DeleteNote
        });
        checkers.Add(new NoteAuthorization
        {
            Requirement = NoteOperations.ReadNoteLogsRequirement,
            Checker = (rights, note) => rights.ReadNoteActionLog
        });
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NoteRequirement requirement, string resource)
    {
        try
        {
            if (await userService.IsAdministrator())
            {
                context.Succeed(requirement);
                return;
            }

            var note = noteService.Get(resource);
            var userId = utils.GetRequiredCurrentUserId();

            if (ObjectHelper.IsNotNull(note.UserId) && note.UserId == userId)
            {
                context.Succeed(requirement);
                return;
            }

            if (ObjectHelper.IsNotNull(note.GroupId))
            {
                if (groupService.IsCurrentOwner((int)note.GroupId))
                {
                    context.Succeed(requirement);
                    return;
                }

                var rights = groupService.GetUserRole((int)note.GroupId);

                if (ObjectHelper.IsNull(rights))
                {
                    context.Fail();
                    return;
                }

                var check = checkers.FirstOrDefault(x => x.Requirement == requirement);

                ObjectHelper.WhenNotNull(check, c =>
                {
                    if (c.Checker(rights, note))
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

    public class NoteAuthorization
    {
        public NoteRequirement Requirement { get; set; } = default!;
        public Func<GroupRole, Note, bool> Checker { get; set; } = (role, note) => true;
    }
}

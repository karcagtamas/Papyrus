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
            Checker = (input) => input.Note.Public || input.GroupRole.ReadNote || input.GroupRole.EditNote || input.GroupRole.DeleteNote
        });
        checkers.Add(new NoteAuthorization
        {
            Requirement = NoteOperations.EditNoteRequirement,
            Checker = (input) => (input.GroupRole.EditNote || input.GroupRole.DeleteNote) && input.GroupEditCheck
        });
        checkers.Add(new NoteAuthorization
        {
            Requirement = NoteOperations.DeleteNoteRequirement,
            Checker = (input) => input.GroupRole.DeleteNote && input.GroupEditCheck
        });
        checkers.Add(new NoteAuthorization
        {
            Requirement = NoteOperations.ReadNoteLogsRequirement,
            Checker = (input) => input.GroupRole.ReadNoteActionLog
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

                GroupRole? rights = null;//groupService.GetUserRole((int)note.GroupId);

                if (ObjectHelper.IsNull(rights))
                {
                    context.Fail();
                    return;
                }

                var group = groupService.Get((int)note.GroupId);
                var check = checkers.FirstOrDefault(x => x.Requirement == requirement);

                ObjectHelper.WhenNotNull(check, c =>
                {
                    if (c.Checker(new NoteAuthorizationInput { GroupRole = rights, Note = note, GroupCase = true, GroupIsClosed = group.IsClosed }))
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
        public Func<NoteAuthorizationInput, bool> Checker { get; set; } = (input) => true;
    }

    public class NoteAuthorizationInput
    {
        public GroupRole GroupRole { get; set; } = default!;
        public Note Note { get; set; } = default!;
        public bool GroupCase { get; set; } = default!;
        public bool GroupIsClosed { get; set; } = default!;

        public bool GroupEditCheck { get => !GroupCase || !GroupIsClosed; }
    }
}

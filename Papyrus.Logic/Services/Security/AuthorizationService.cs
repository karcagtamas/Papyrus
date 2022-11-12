using KarcagS.Shared.Helpers;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Logic.Services.Security.Interfaces;
using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Services.Security;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserService userService;
    private readonly IApplicationService applicationService;
    private readonly IGroupService groupService;
    private readonly INoteService noteService;
    private readonly IFolderService folderService;
    private readonly ITagService tagService;

    public AuthorizationService(
        IUserService userService,
        IApplicationService applicationService,
        IGroupService groupService,
        INoteService noteService,
        IFolderService folderService,
        ITagService tagService
        )
    {
        this.userService = userService;
        this.applicationService = applicationService;
        this.groupService = groupService;
        this.noteService = noteService;
        this.folderService = folderService;
        this.tagService = tagService;
    }

    public Task<bool> UserHasApplicationAccessRight(string userId, string appId)
    {
        return WithUser(userId, (user) =>
        {
            var app = applicationService.GetOptional(appId);

            if (ObjectHelper.IsNull(app))
            {
                return false;
            }

            return app.UserId == user.Id;
        });
    }

    public Task<bool> UserHasGroupRight(string userId, int groupId, GroupRight right)
    {
        return WithGroup(userId, groupId, (user, group) =>
        {
            var role = group.Members.FirstOrDefault(x => x.UserId == user.Id)?.Role;

            if (ObjectHelper.IsNull(role))
            {
                return false;
            }

            return CheckUserGroupRight(right, group, role);
        });
    }

    public static bool CheckUserGroupRight(GroupRight right, Group group, GroupRole role, bool fullAccess = false)
    {
        return right switch
        {
            GroupRight.Read => true,
            GroupRight.Edit => (fullAccess || role.GroupEdit) && !group.IsClosed,
            GroupRight.Close => fullAccess && !group.IsClosed,
            GroupRight.Open => fullAccess && group.IsClosed,
            GroupRight.Remove => fullAccess,
            GroupRight.ReadLogs => fullAccess || role.ReadGroupActionLog,
            GroupRight.ReadRoles => fullAccess || role.ReadRoleList || role.EditRoleList,
            GroupRight.EditRoles => (fullAccess || role.EditRoleList) && !group.IsClosed,
            GroupRight.ReadMembers => fullAccess || role.ReadMemberList || role.EditMemberList,
            GroupRight.EditMembers => (fullAccess || role.EditMemberList) && !group.IsClosed,
            GroupRight.ReadNote => fullAccess || role.ReadNote || role.EditNote || role.DeleteNote,
            GroupRight.ReadNotes => fullAccess || role.ReadNoteList || role.ReadNote || role.EditNote || role.DeleteNote,
            GroupRight.ManageNote => (fullAccess || role.EditNote || role.DeleteNote) && !group.IsClosed,
            GroupRight.CreateFolder => (fullAccess || role.EditNote || role.DeleteNote) && !group.IsClosed,
            GroupRight.ReadTags => fullAccess || role.ReadTagList || role.EditTagList,
            GroupRight.ManageTag => (fullAccess || role.EditTagList) && !group.IsClosed,
            _ => false,
        };
    }

    public Task<bool> UserHasNoteRight(string userId, string noteId, NoteRight right)
    {
        return WithNote(userId, noteId, (user, note, group) =>
        {
            var role = group.Members.FirstOrDefault(x => x.UserId == user.Id)?.Role;

            if (ObjectHelper.IsNull(role))
            {
                return false;
            }

            return CheckUserGroupNoteRight(right, note, group, role);
        });
    }

    public static bool CheckUserGroupNoteRight(NoteRight right, Note note, Group group, GroupRole role, bool fullAccess = false)
    {
        return right switch
        {
            NoteRight.Read => fullAccess || note.Public || role.ReadNote || role.EditNote || role.DeleteNote,
            NoteRight.Edit => (fullAccess || role.EditNote || role.DeleteNote) && !group.IsClosed,
            NoteRight.Delete => (fullAccess || role.DeleteNote) && !group.IsClosed,
            NoteRight.ReadLogs => fullAccess || role.ReadNoteActionLog,
            _ => false,
        };
    }

    public Task<bool> UserHasFolderRight(string userId, string folderId, FolderRight right)
    {
        return WithFolder(userId, folderId, (user, folder, group) =>
        {
            var role = group.Members.FirstOrDefault(x => x.UserId == user.Id)?.Role;

            if (ObjectHelper.IsNull(role))
            {
                return false;
            }

            return CheckUserGroupFolderRight(right, group, role);
        });
    }

    public static bool CheckUserGroupFolderRight(FolderRight right, Group group, GroupRole role)
    {
        return right switch
        {
            FolderRight.Read => role.ReadNoteList || role.ReadNote || role.EditNote || role.DeleteNote,
            FolderRight.Manage => (role.EditNote || role.DeleteNote) && !group.IsClosed,
            _ => false,
        };
    }

    public Task<bool> UserHasTagRight(string userId, int tagId, TagRight right)
    {
        return WithTag(userId, tagId, (user, tag, group) =>
        {
            var role = group.Members.FirstOrDefault(x => x.UserId == user.Id)?.Role;

            if (ObjectHelper.IsNull(role))
            {
                return false;
            }

            return CheckUserGroupTagRight(right, group, role);
        });
    }

    public static bool CheckUserGroupTagRight(TagRight right, Group group, GroupRole role)
    {
        return right switch
        {
            TagRight.Read => role.ReadTagList || role.EditTagList,
            TagRight.Edit => role.EditTagList && !group.IsClosed,
            _ => false,
        };
    }

    private async Task<bool> WithUser(string userId, Func<User, bool> func)
    {
        var user = userService.Get(userId);

        var isAdmin = await userService.IsUserAdministrator(user);

        return isAdmin || func(user);
    }

    private Task<bool> WithGroup(string userId, int groupId, Func<User, Group, bool> func)
    {
        return WithUser(userId, (user) =>
        {
            return WithGroup(user, groupId, func);
        });
    }

    private bool WithGroup(User user, int groupId, Func<User, Group, bool> func)
    {
        var group = groupService.GetOptional(groupId);

        if (ObjectHelper.IsNull(group))
        {
            return false;
        }

        return groupService.IsUserOwner(group.Id, user) || func(user, group);
    }

    private Task<bool> WithNote(string userId, string noteId, Func<User, Note, Group, bool> func)
    {
        return WithUser(userId, (user) =>
        {
            var note = noteService.GetOptional(noteId);

            if (ObjectHelper.IsNull(note))
            {
                return false;
            }

            if (note.UserId == userId)
            {
                return true;
            }

            if (ObjectHelper.IsNotNull(note.GroupId))
            {
                return WithGroup(user, (int)note.GroupId, (user, group) => func(user, note, group));
            }

            return false;
        });
    }

    private Task<bool> WithFolder(string userId, string folderId, Func<User, Folder, Group, bool> func)
    {
        return WithUser(userId, (user) =>
        {
            var folder = folderService.GetOptional(folderId);

            if (ObjectHelper.IsNull(folder))
            {
                return false;
            }

            if (folder.UserId == userId)
            {
                return true;
            }

            if (ObjectHelper.IsNotNull(folder.GroupId))
            {
                return WithGroup(user, (int)folder.GroupId, (user, group) => func(user, folder, group));
            }

            return false;
        });
    }

    private Task<bool> WithTag(string userId, int tagId, Func<User, Tag, Group, bool> func)
    {
        return WithUser(userId, (user) =>
        {
            var tag = tagService.GetOptional(tagId);

            if (ObjectHelper.IsNull(tag))
            {
                return false;
            }

            if (tag.UserId == userId)
            {
                return true;
            }

            if (ObjectHelper.IsNotNull(tag.GroupId))
            {
                return WithGroup(user, (int)tag.GroupId, (user, group) => func(user, tag, group));
            }

            return false;
        });
    }
}

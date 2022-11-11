using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Services.Security.Interfaces;

public interface IAuthorizationService
{
    public Task<bool> UserHasApplicationAccessRight(string userId, string appId);
    public Task<bool> UserHasGroupRight(string userId, int groupId, GroupRight right);
    public Task<bool> UserHasNoteRight(string userId, string noteId, NoteRight right);
    public Task<bool> UserHasFolderRight(string userId, string folderId, FolderRight right);
    public Task<bool> UserHasTagRight(string userId, int tagId, TagRight right);
}

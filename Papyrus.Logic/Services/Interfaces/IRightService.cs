namespace Papyrus.Logic.Services.Interfaces;

public interface IRightService
{
    Task<bool> HasGroupReadRight(int groupId);
    Task<bool> HasGroupRemoveRight(int groupId);
    Task<bool> HasGroupCloseOpenRight(int groupId);
    Task<bool> HasGroupEditRight(int groupId);
    Task<bool> HasGroupLogListReadRight(int groupId);
    Task<bool> HasGroupMemberListReadRight(int groupId);
    Task<bool> HasGroupMemberListEditRight(int groupId);
    Task<bool> HasGroupRoleListReadRight(int groupId);
    Task<bool> HasGroupRoleListEditRight(int groupId);
    Task<bool> HasGroupNoteCreateRight(int groupId);
    Task<bool> HasGroupFolderCreateRight(int groupId);
    Task<bool> HasGroupNoteListReadRight(int groupId);
    Task<bool> HasGroupTagListReadRight(int groupId);
    Task<bool> HasGroupTagCreateRight(int groupId);
    Task<bool> HasNoteReadRight(string noteId);
    Task<bool> HasNoteEditRight(string noteId);
    Task<bool> HasNoteDeleteRight(string noteId);
    Task<bool> HasNoteLogListReadRight(string noteId);
    Task<bool> HasTagEditRight(int tagId);
    Task<bool> HasTagReadRight(int tagId);
    Task<bool> HasFolderReadRight(string folderId);
    Task<bool> HasFolderManageRight(string folderId);
}

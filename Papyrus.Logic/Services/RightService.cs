using KarcagS.Common.Tools.Services;
using Microsoft.AspNetCore.Authorization;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Logic.Services;

public class RightService : IRightService
{
    private readonly IAuthorizationService authorization;
    private readonly IUtilsService<string> utils;

    public RightService(IAuthorizationService authorization, IUtilsService<string> utils)
    {
        this.utils = utils;
        this.authorization = authorization;
    }

    public Task<bool> HasApplicationAccessRight(string appId) => Check(appId, ApplicationPolicies.AccessApplication);

    public Task<bool> HasFolderManageRight(string folderId) => Check(folderId, FolderPolicies.ManagerFolder);

    public Task<bool> HasFolderReadRight(string folderId) => Check(folderId, FolderPolicies.ReadFolder);

    public Task<bool> HasGroupCloseOpenRight(int groupId) => Check(groupId, GroupPolicies.CloseOpenGroup);

    public Task<bool> HasGroupEditRight(int groupId) => Check(groupId, GroupPolicies.EditGroup);

    public Task<bool> HasGroupFolderCreateRight(int groupId) => Check(groupId, GroupPolicies.CreateFolder);

    public Task<bool> HasGroupLogListReadRight(int groupId) => Check(groupId, GroupPolicies.ReadGroupLogs);

    public Task<bool> HasGroupMemberListEditRight(int groupId) => Check(groupId, GroupPolicies.EditGroupMembers);

    public Task<bool> HasGroupMemberListReadRight(int groupId) => Check(groupId, GroupPolicies.ReadGroupMembers);

    public Task<bool> HasGroupNoteCreateRight(int groupId) => Check(groupId, GroupPolicies.CreateNote);

    public Task<bool> HasGroupNoteListReadRight(int groupId) => Check(groupId, GroupPolicies.ReadNotes);

    public Task<bool> HasGroupReadRight(int groupId) => Check(groupId, GroupPolicies.ReadGroup);

    public Task<bool> HasGroupRemoveRight(int groupId) => Check(groupId, GroupPolicies.RemoveGroup);

    public Task<bool> HasGroupRoleListEditRight(int groupId) => Check(groupId, GroupPolicies.EditGroupRoles);

    public Task<bool> HasGroupRoleListReadRight(int groupId) => Check(groupId, GroupPolicies.ReadGroupRoles);

    public Task<bool> HasGroupTagCreateRight(int groupId) => Check(groupId, GroupPolicies.CreateTag);

    public Task<bool> HasGroupTagListReadRight(int groupId) => Check(groupId, GroupPolicies.ReadTags);

    public Task<bool> HasNoteDeleteRight(string noteId) => Check(noteId, NotePolicies.DeleteNote);

    public Task<bool> HasNoteEditRight(string noteId) => Check(noteId, NotePolicies.EditNote);

    public Task<bool> HasNoteLogListReadRight(string noteId) => Check(noteId, NotePolicies.ReadNoteLogs);

    public Task<bool> HasNoteReadRight(string noteId) => Check(noteId, NotePolicies.ReadNote);

    public Task<bool> HasTagEditRight(int tagId) => Check(tagId, TagPolicies.EditTag);

    public Task<bool> HasTagReadRight(int tagId) => Check(tagId, TagPolicies.ReadTag);

    private async Task<bool> Check(object resource, AuthorizationPolicy policy)
    {
        var result = await authorization.AuthorizeAsync(utils.GetRequiredUserPrincipal(), resource, policy.Requirements);

        return result.Succeeded;
    }
}

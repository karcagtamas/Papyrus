using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupRoleService : MapperRepository<GroupRole, int, string>, IGroupRoleService
{
    public GroupRoleService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Group Role")
    {
    }

    public void CreateDefaultRoles(int groupId)
    {
        CreateAdminRole(groupId);
        CreateModeratorRole(groupId);
        CreateDefaultRole(groupId);
    }

    public List<GroupRoleDTO> GetGroupList(int groupId)
    {
        return GetMappedList<GroupRoleDTO>(x => x.GroupId == groupId)
            .OrderBy(x => x.ReadOnly)
            .ToList();
    }

    private void CreateAdminRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Administrator",
            ReadOnly = true,
            GroupEdit = true,
            GroupClose = true,
            ReadNoteList = true,
            ReadNote = true,
            CreateNote = true,
            DeleteNote = true,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = true,
            ReadRoleList = true,
            EditRoleList = true,
            ReadGroupActionLog = true,
            ReadNoteActionLog = true,
            ReadTagList = true,
            EditTagList = true
        };

        Create(role);
    }

    private void CreateModeratorRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Moderator",
            ReadOnly = true,
            GroupEdit = true,
            GroupClose = false,
            ReadNoteList = true,
            ReadNote = true,
            CreateNote = true,
            DeleteNote = true,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = false,
            ReadRoleList = true,
            EditRoleList = false,
            ReadGroupActionLog = true,
            ReadNoteActionLog = true,
            ReadTagList = true,
            EditTagList = true
        };

        Create(role);
    }

    private void CreateDefaultRole(int groupId)
    {
        var role = new GroupRole
        {
            GroupId = groupId,
            Name = "Default",
            ReadOnly = true,
            GroupEdit = false,
            GroupClose = false,
            ReadNoteList = true,
            ReadNote = true,
            CreateNote = true,
            DeleteNote = false,
            EditNote = true,
            ReadMemberList = true,
            EditMemberList = false,
            ReadRoleList = false,
            EditRoleList = false,
            ReadGroupActionLog = false,
            ReadNoteActionLog = false,
            ReadTagList = true,
            EditTagList = false
        };

        Create(role);
    }


}

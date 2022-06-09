using AutoMapper;
using Papyrus.Shared.Models.Groups;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Mappers;

public class GroupMapper : Profile
{
    public GroupMapper()
    {
        CreateMap<GroupModel, Group>();
        CreateMap<Group, GroupDTO>();
        CreateMap<Group, GroupListDTO>();

        CreateMap<GroupActionLog, GroupActionLogDTO>();
        CreateMap<GroupMember, GroupMemberDTO>();
        CreateMap<GroupRole, GroupRoleDTO>();
        CreateMap<GroupRole, GroupRoleLightDTO>();
        CreateMap<GroupMemberModel, GroupMember>();
        CreateMap<GroupRoleModel, GroupRole>();
    }
}

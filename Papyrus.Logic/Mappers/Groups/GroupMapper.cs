using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Logic.Mappers.Groups;

public class GroupMapper : AutoMapper.Profile
{
    public GroupMapper()
    {
        CreateMap<GroupModel, Group>();
        CreateMap<Group, GroupDTO>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Count))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes.Count))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Count));
        CreateMap<Group, GroupListDTO>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.UserName));

        CreateMap<GroupMember, GroupMemberDTO>()
            .ForMember(dest => dest.Join, opt => opt.MapFrom(src => src.Creation));
        CreateMap<GroupRole, GroupRoleDTO>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members.Count));
        CreateMap<GroupRole, GroupRoleLightDTO>();
        CreateMap<GroupMemberCreateModel, GroupMember>();
        CreateMap<GroupMemberUpdateModel, GroupMember>();
        CreateMap<GroupRoleModel, GroupRole>();
    }
}

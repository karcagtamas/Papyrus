using AutoMapper;
using Papyrus.Shared.Models.Groups;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;
using KarcagS.Shared.Helpers;

namespace Papyrus.Logic.Mappers;

public class GroupMapper : Profile
{
    public GroupMapper()
    {
        CreateMap<GroupModel, Group>();
        CreateMap<Group, GroupDTO>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Count))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes.Count))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Count))
            .ForMember(dest => dest.LastAction, opt => opt.MapFrom(src => GetLastAction(src.ActionLogs.ToList())));
        CreateMap<Group, GroupListDTO>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.UserName));

        CreateMap<GroupActionLog, GroupActionLogDTO>()
            .ForMember(dest => dest.Performer, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.Performer, p => p.UserName) ?? "System"));
        CreateMap<GroupMember, GroupMemberDTO>()
            .ForMember(dest => dest.Join, opt => opt.MapFrom(src => src.Creation));
        CreateMap<GroupRole, GroupRoleDTO>();
        CreateMap<GroupRole, GroupRoleLightDTO>();
        CreateMap<GroupMemberCreateModel, GroupMember>();
        CreateMap<GroupMemberUpdateModel, GroupMember>();
        CreateMap<GroupRoleModel, GroupRole>();
    }

    private static DateTime? GetLastAction(List<GroupActionLog> actionLogs)
    {
        var log = actionLogs.OrderBy(x => x.Creation).LastOrDefault();

        return log?.Creation;
    }
}

using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Mappers;

public class RoleMapper : AutoMapper.Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleDTO>()
            .ForMember(dest => dest.NameEN, opt => opt.MapFrom(src => src.Name));
    }
}

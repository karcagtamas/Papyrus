using AutoMapper;
using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleDTO>();
    }
}

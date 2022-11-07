using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.DTOs.Auth;
using Papyrus.Shared.Models;

namespace Papyrus.Logic.Mappers;

public class UserMapper : AutoMapper.Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDTO>();
        CreateMap<User, UserListDTO>();
        CreateMap<User, UserTokenDTO>();
        CreateMap<User, UserLightDTO>();
        CreateMap<UserModel, User>();

        CreateMap<AppAccess, AccessDTO>();
    }
}

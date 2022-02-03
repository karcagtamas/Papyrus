using AutoMapper;
using NoteWeb.DataAccess.Entities;
using NoteWeb.Shared.DTOs;
using NoteWeb.Shared.Models;

namespace NoteWeb.Logic.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDTO>();
        CreateMap<User, UserListDTO>();
        CreateMap<User, UserTokenDTO>();
        CreateMap<UserModel, User>();
    }
}

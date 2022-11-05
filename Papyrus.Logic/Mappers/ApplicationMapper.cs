using AutoMapper;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Logic.Mappers;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<Application, ApplicationDTO>();
        CreateMap<ApplicationModel, Application>();
    }
}

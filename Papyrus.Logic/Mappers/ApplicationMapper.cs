using AutoMapper;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Shared.DTOs.Profile;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Mappers;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<Application, ApplicationDTO>();
        CreateMap<ApplicationModel, Application>();
    }
}

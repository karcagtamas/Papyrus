using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Logic.Services.Profile.Interfaces;

namespace Papyrus.Logic.Services.Profile;

public class ApplicationService : MapperRepository<Application, string, string>, IApplicationService
{
    public ApplicationService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Application")
    {
    }
}

using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Logic.Services.Profile.Interfaces;
using Papyrus.Shared.Models;

namespace Papyrus.Logic.Services.Profile;

public class ApplicationService : MapperRepository<Application, string, string>, IApplicationService
{
    public ApplicationService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Application")
    {
    }

    public void CreateWithKeys(ApplicationModel model)
    {
        var entity = Mapper.Map<Application>(model);

        entity.PublicId = Guid.NewGuid().ToString();
        entity.SecretId = Guid.NewGuid().ToString();

        Create(entity);
    }

    public void RefreshSecret(string id)
    {
        var app = Get(id);

        app.SecretId = Guid.NewGuid().ToString();

        Update(app);
    }
}

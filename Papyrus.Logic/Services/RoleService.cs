using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Logic.Services;

public class RoleService : MapperRepository<Role, string, string>, IRoleService
{
    public RoleService(PapyrusContext context, ILoggerService logger, IUtilsService<string> utils, IMapper mapper) : base(context, logger, utils, mapper, "Role")
    {
    }
}

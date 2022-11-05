using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities;

namespace Papyrus.Logic.Services.Interfaces;

public interface IApplicationService : IMapperRepository<Application, string>
{
}

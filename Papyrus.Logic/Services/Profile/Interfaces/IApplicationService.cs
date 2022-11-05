using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Profile;

namespace Papyrus.Logic.Services.Profile.Interfaces;

public interface IApplicationService : IMapperRepository<Application, string>
{
}

using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Profile;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Services.Profile.Interfaces;

public interface IApplicationService : IMapperRepository<Application, string>
{
    void CreateWithKeys(ApplicationModel model);
    void RefreshSecret(string id);
}

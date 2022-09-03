using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Services.Interfaces;

public interface IRoleService : IMapperRepository<Role, string>
{
    List<RoleDTO> GetAllTranslated(string? lang = null);
}

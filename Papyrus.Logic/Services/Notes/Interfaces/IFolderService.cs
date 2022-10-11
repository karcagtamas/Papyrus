using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface IFolderService : IMapperRepository<Folder, string>
{
    void createRootFolder(string? userId = null, int? groupId = null);
}

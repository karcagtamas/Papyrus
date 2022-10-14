using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface IFolderService : IMapperRepository<Folder, string>
{
    void CreateRootFolder(string? userId = null, int? groupId = null);
    void CreateFolder(FolderModel model);
    void EditFolder(string id, FolderModel model);
    FolderContentDTO GetContent(string? folderId, int? groupId);
}

using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface ITagService : IMapperRepository<Tag, int>
{
    List<NoteTagDTO> GetList(int? groupId);
    List<TagTreeItemDTO> GetTree(int? groupId, int? filteredTag = null);
    TagPathDTO GetPath(int id);
}

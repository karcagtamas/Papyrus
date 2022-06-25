using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface ITagService : IMapperRepository<Tag, int>
{
    List<TagDTO> GetByGroup(int groupId);
    List<GroupTagTreeItemDTO> GetTreeByGroup(int groupId);
    TagPathDTO GetPath(int id);
}

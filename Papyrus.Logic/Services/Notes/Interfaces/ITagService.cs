using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface ITagService : IMapperRepository<Tag, int>
{
    List<GroupTagDTO> GetByGroup(int groupId);
    List<GroupTagTreeItemDTO> GetTreeByGroup(int groupId);
}

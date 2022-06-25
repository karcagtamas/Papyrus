using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface ITagService : IHttpCall<int>
{
    Task<List<TagDTO>> GetByGroup(int groupId);
    Task<List<GroupTagTreeItemDTO>> GetTreeByGroup(int groupId);
    Task<bool> CreateGroupTag(GroupTagModel model);
    Task<bool> UpdateGroupTag(int id, GroupTagModel model);
    Task<TagPathDTO> GetPath(int id);
}

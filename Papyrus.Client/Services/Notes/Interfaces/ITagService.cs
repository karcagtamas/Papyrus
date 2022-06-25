using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface ITagService : IHttpCall<int>
{
    Task<List<GroupTagDTO>> GetByGroup(int groupId);
    Task<List<GroupTagTreeItemDTO>> GetTreeByGroup(int groupId);
}

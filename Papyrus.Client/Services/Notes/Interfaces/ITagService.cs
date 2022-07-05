using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface ITagService : IHttpCall<int>
{
    Task<List<TagDTO>> GetByGroup(int groupId);
    Task<List<TagTreeItemDTO>> GetTree(int? groupId = null, int? filteredTag = null);
    Task<TagPathDTO> GetPath(int id);
}

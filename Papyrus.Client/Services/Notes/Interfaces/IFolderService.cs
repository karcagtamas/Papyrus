using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface IFolderService : IHttpCall<string>
{
    Task<FolderContentDTO> GetContent(string? folder, int? groupId);
    Task<bool> Exists(string parentFolderId, string name);
}

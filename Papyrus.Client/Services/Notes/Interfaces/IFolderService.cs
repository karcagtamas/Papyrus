using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface IFolderService : IHttpCall<string>
{
    Task<FolderContentDTO> GetContent(string? folder, int? groupId);
}

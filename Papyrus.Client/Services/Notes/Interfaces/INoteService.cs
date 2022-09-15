using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface INoteService : IHttpCall<string>
{
    Task<List<NoteLightDTO>> GetByGroup(int groupId, NotePublishType publishType = NotePublishType.All, bool archiveStatus = false);
    Task<List<NoteLightDTO>> GetByUser(NotePublishType publishType = NotePublishType.All, bool archiveStatus = false);
    Task<NoteCreationDTO?> CreateEmpty(int? groupId);
    Task<NoteLightDTO?> GetLight(string id);
    Task<NoteRightsDTO> GetRights(string id);
}

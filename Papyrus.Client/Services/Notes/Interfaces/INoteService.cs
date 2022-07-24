using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface INoteService : IHttpCall<string>
{
    Task<List<NoteLightDTO>> GetByGroup(int groupId);
    Task<List<NoteLightDTO>> GetByUser();
    Task<NoteCreationDTO?> CreateEmpty(int? groupId);
}

using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Services.Notes.Interfaces;

public interface INoteService : IHttpCall<string>
{
    Task<List<NoteLightDTO>> GetFiltered(NoteFilterQueryModel query, int? groupId);
    Task<NoteCreationDTO?> CreateEmpty(int? groupId);
    Task<NoteLightDTO?> GetLight(string id);
    Task<NoteRightsDTO> GetRights(string id);
    Task<List<SearchResultDTO>> Search(SearchQueryModel query);
}

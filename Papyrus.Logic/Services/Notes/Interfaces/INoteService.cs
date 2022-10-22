using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteService : IMapperRepository<Note, string>
{
    List<NoteLightDTO> GetFiltered(NoteFilterQueryModel query, int? groupId);
    NoteCreationDTO CreateEmpty(NoteCreateModel model);
    Task<NoteRightsDTO> GetRights(string id);
    void UpdateWithTags(string id, NoteModel model);
    List<SearchResultDTO> Search(SearchQueryModel query);
    void DeleteFolder(string folderId);
    bool Exists(string parentFolderId, string title, string? id);
    void Access(string id);
}

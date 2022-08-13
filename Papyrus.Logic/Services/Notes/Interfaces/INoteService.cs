using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteService : IMapperRepository<Note, string>
{
    List<NoteLightDTO> GetByGroup(int groupId, NoteSearchType searchType = NoteSearchType.All);
    List<NoteLightDTO> GetByUser(NoteSearchType searchType = NoteSearchType.All);
    NoteCreationDTO CreateEmpty(int? groupId);
    void UpdateWithTags(string id, NoteModel model);
    void Delete(string id);
}

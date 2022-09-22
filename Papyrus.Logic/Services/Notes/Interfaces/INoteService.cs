using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteService : IMapperRepository<Note, string>
{
    List<NoteLightDTO> GetByGroup(int groupId, NoteFilterQueryModel query);
    List<NoteLightDTO> GetByUser(NoteFilterQueryModel query);
    NoteCreationDTO CreateEmpty(NoteCreateModel model);
    Task<NoteRightsDTO> GetRights(string id);
    void UpdateWithTags(string id, NoteModel model);
    void Delete(string id);
}

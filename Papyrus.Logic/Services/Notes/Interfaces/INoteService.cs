using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Enums.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteService : IMapperRepository<Note, string>
{
    List<NoteLightDTO> GetByGroup(int groupId, NotePublishType publishType = NotePublishType.All, bool archiveStatus = false);
    List<NoteLightDTO> GetByUser(NotePublishType publishType = NotePublishType.All, bool archiveStatus = false);
    NoteCreationDTO CreateEmpty(NoteCreateModel model);
    Task<NoteRightsDTO> GetRights(string id);
    void UpdateWithTags(string id, NoteModel model);
    void Delete(string id);
}

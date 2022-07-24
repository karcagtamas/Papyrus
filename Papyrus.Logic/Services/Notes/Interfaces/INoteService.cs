using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteService : IMapperRepository<Note, string>
{
    List<NoteLightDTO> GetByGroup(int groupId);
    List<NoteLightDTO> GetByUser();
    NoteCreationDTO CreateEmpty(int? groupId);
}

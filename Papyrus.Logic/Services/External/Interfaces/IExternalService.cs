using Papyrus.Shared.DTOs.External;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Services.External.Interfaces;

public interface IExternalService
{
    List<NoteExtDTO> GetNotes(ApplicationQueryModel query);
    NoteContentExtDTO GetNote(ApplicationQueryModel query, string id);
    List<GroupListExtDTO> GetGroups(ApplicationQueryModel query);
    GroupExtDTO GetGroup(ApplicationQueryModel query, int id);
    List<TagTreeExtDTO> GetTagsInTree(ApplicationQueryModel query);
    List<TagExtDTO> GetTagsInList(ApplicationQueryModel query);
}

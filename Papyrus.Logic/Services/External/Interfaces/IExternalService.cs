using Papyrus.Shared.DTOs.External;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Services.External.Interfaces;

public interface IExternalService
{
    List<NoteExtDTO> GetNotes(ApplicationQueryModel query);
    NoteContentExtDTO GetNote(ApplicationQueryModel query, string noteId);
    List<T> GetTags<T>(ApplicationQueryModel query, bool inTree = false) where T : TagExtDTO;
    List<GroupListExtDTO> GetGroups(ApplicationQueryModel query);
    GroupExtDTO GetGroup(ApplicationQueryModel query, int groupId);
    List<NoteExtDTO> GetGroupNotes(ApplicationQueryModel query, int groupId);
    List<NoteExtDTO> GetGroupNote(ApplicationQueryModel query, int groupId, string noteId);
    List<T> GetGroupTags<T>(ApplicationQueryModel query, int groupId, bool inTree = false) where T : TagExtDTO;
    List<object> GetGroupMembers(ApplicationQueryModel query, int groupId);

}

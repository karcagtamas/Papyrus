using Papyrus.Shared.DTOs.External;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Services.External.Interfaces;

public interface IExternalService
{
    List<NoteExtDTO> GetNotes(ApplicationQueryModel query);
    List<GroupExtDTO> GetGroups(ApplicationQueryModel query);
    List<TagTreeExtDTO> GetTagsInTree(ApplicationQueryModel query);
    List<TagExtDTO> GetTagsInList(ApplicationQueryModel query);
}

using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Services.Editor.Interfaces;

public interface IEditorService
{
    void AddMember(string userId, string noteId, string connectionId);
    void RemoveMember(string userId, string noteId, string connectionId);
    List<UserLightDTO> GetMembers(string noteId);
}

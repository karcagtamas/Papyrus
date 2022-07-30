using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services.Editor.Interfaces;

public interface IEditorService
{
    Task<List<UserLightDTO>> GetMembers(string id);
}

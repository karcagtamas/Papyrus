namespace Papyrus.Shared.DTOs.Notes;

public class FolderContentDTO
{
    public List<FolderDTO> Folders { get; set; } = new();
    public List<NoteLightDTO> Notes { get; set; } = new();
}

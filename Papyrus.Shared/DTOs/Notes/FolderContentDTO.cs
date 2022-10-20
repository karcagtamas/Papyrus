namespace Papyrus.Shared.DTOs.Notes;

public class FolderContentDTO
{
    public FolderDTO ParentFolder { get; set; } = default!;
    public List<FolderDTO> Folders { get; set; } = new();
    public List<NoteLightDTO> Notes { get; set; } = new();
    public List<FolderPathDTO> Path { get; set; } = new();
}

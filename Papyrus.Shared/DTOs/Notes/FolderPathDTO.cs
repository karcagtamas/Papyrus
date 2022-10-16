namespace Papyrus.Shared.DTOs.Notes;

public class FolderPathDTO
{
    public string FolderId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsRoot { get; set; }
}

namespace Papyrus.Shared.DTOs.External;

public class TagTreeExtDTO : TagExtDTO
{
    public List<TagTreeExtDTO> Children { get; set; } = new();
}

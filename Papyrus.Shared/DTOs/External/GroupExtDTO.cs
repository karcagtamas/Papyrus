namespace Papyrus.Shared.DTOs.External;

public class GroupExtDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime Creation { get; set; }
    public bool IsClosed { get; set; }
    public string Url { get; set; } = default!;
    public string NotesUrl { get; set; } = default!;
    public string TagsUrl { get; set; } = default!;
    public string MembersUrl { get; set; } = default!;
}

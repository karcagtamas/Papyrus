namespace Papyrus.Shared.DTOs.External;

public class GroupExtDTO : GroupListExtDTO
{
    public bool IsClosed { get; set; }
    public string NotesUrl { get; set; } = default!;
    public string TagsUrl { get; set; } = default!;
    public string MembersUrl { get; set; } = default!;
}

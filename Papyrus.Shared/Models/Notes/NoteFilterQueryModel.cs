namespace Papyrus.Shared.Models.Notes;

public class NoteFilterQueryModel
{
    public bool? PublicStatus { get; set; } = null;
    public bool? ArchivedStatus { get; set; } = null;
    public string? TextFilter { get; set; } = null;
    public DateTime? DateFilter { get; set; } = null;
    public List<int> Tags { get; set; } = new();
}

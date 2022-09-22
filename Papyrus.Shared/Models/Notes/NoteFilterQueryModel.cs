using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Shared.Models.Notes;

public class NoteFilterQueryModel
{
    public NotePublishType PublishType { get; set; } = NotePublishType.All;
    public bool ArchivedStatus { get; set; } = false;
    public string? TextFilter { get; set; } = null;
    public DateTime? DateFilter { get; set; } = null;
    public List<int> Tags { get; set; } = new();
}

using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Notes;

public class SearchQueryModel
{
    [Required]
    public string Text { get; set; } = default!;

    public bool IncludeTags { get; set; } = true;
    public bool IncludeContents { get; set; }
    public bool OnlyPublics { get; set; }

    public bool Archived { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

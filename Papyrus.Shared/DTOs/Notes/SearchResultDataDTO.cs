using Papyrus.Shared.Enums.Notes;

namespace Papyrus.Shared.DTOs.Notes;

public class SearchResultDataDTO
{
    public SearchResultCategory Type { get; set; }
    public string Owner { get; set; } = default!;
    public bool OwnerIsGroup { get; set; }
    public bool Openable { get; set; }
}

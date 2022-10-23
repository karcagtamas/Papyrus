namespace Papyrus.Shared.DTOs.Notes;

public class NoteDashboardDTO
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public bool CanOpen { get; set; } = default!;
}

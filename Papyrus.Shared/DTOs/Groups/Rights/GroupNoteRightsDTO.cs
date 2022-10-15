namespace Papyrus.Shared.DTOs.Groups.Rights;

public class GroupNoteRightsDTO
{
    public bool CanCreateNote { get; set; }
    public bool CanViewNote { get; set; }
    public bool CanOpenNote { get; set; }
    public bool CanCreateFolder { get; set; }
    public bool CanManageFolder { get; set; }
    public bool CanEditNote { get; set; }
    public bool CanDeleteNote { get; set; }
}

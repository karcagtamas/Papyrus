namespace Papyrus.Shared.DTOs.Groups.Rights;

public class GroupRightsDTO
{
    public bool CanClose { get; set; }
    public bool CanOpen { get; set; }
    public bool CanRemove { get; set; }
    public bool CanEdit { get; set; }
    public bool CanOpenNote { get; set; }

    public GroupRightsDTO()
    {

    }

    public GroupRightsDTO(bool value)
    {
        CanClose = value;
        CanOpen = value;
        CanRemove = value;
        CanEdit = value;
        CanOpenNote = value;
    }
}

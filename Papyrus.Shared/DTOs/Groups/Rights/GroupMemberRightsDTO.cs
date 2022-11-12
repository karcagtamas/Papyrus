namespace Papyrus.Shared.DTOs.Groups.Rights;

public class GroupMemberRightsDTO
{
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanView { get; set; }

    public GroupMemberRightsDTO()
    {

    }

    public GroupMemberRightsDTO(bool value)
    {
        CanAdd = value;
        CanEdit = value;
        CanView = value;
    }
}

namespace Papyrus.Shared.DTOs.Groups.Rights;

public class GroupPageRightsDTO
{
    public bool LogPageEnabled { get; set; }
    public bool RolePageEnabled { get; set; }
    public bool MemberPageEnabled { get; set; }

    public GroupPageRightsDTO()
    {

    }

    public GroupPageRightsDTO(bool value)
    {
        LogPageEnabled = value;
        RolePageEnabled = value;
        MemberPageEnabled = value;
    }
}

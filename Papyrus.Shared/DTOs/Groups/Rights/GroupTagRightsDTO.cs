namespace Papyrus.Shared.DTOs.Groups.Rights;

public class GroupTagRightsDTO
{
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanRemove { get; set; }
    public bool CanView { get; set; }

    public GroupTagRightsDTO()
    {

    }

    public GroupTagRightsDTO(bool value)
    {
        CanCreate = value;
        CanEdit = value;
        CanRemove = value;
        CanView = value;
    }
}

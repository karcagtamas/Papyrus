namespace Papyrus.Shared.DTOs.Notes;

public class NoteRightsDTO
{
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanViewLogs { get; set; }

    public NoteRightsDTO()
    {

    }

    public NoteRightsDTO(bool value)
    {
        CanView = value;
        CanEdit = value;
        CanDelete = value;
        CanViewLogs = value;
    }
}

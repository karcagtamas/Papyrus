namespace Papyrus.Shared.DTOs.Groups;

public class GroupRoleDTO : GroupRoleLightDTO
{
    public bool ReadOnly { get; set; }

    public bool GroupEdit { get; set; }

    public bool ReadNoteList { get; set; }
    public bool ReadNote { get; set; }
    public bool DeleteNote { get; set; }
    public bool EditNote { get; set; }

    public bool ReadMemberList { get; set; }
    public bool EditMemberList { get; set; }
    public bool ReadRoleList { get; set; }
    public bool EditRoleList { get; set; }

    public bool ReadGroupActionLog { get; set; }
    public bool ReadNoteActionLog { get; set; }

    public bool ReadTagList { get; set; }
    public bool EditTagList { get; set; }
}

using Papyrus.Shared.DTOs.Groups;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Groups;

public class GroupRoleModel
{
    [Required(ErrorMessage = "Field is required")]
    public string Name { get; set; } = default!;

    [Required]
    public int GroupId { get; set; }


    [Required]
    public bool GroupEdit { get; set; }

    [Required]
    public bool GroupClose { get; set; }



    [Required]
    public bool ReadNoteList { get; set; }

    [Required]
    public bool ReadNote { get; set; }

    [Required]
    public bool CreateNote { get; set; }

    [Required]
    public bool DeleteNote { get; set; }

    [Required]
    public bool EditNote { get; set; }



    [Required]
    public bool ReadMemberList { get; set; }

    [Required]
    public bool EditMemberList { get; set; }



    [Required]
    public bool ReadRoleList { get; set; }

    [Required]
    public bool EditRoleList { get; set; }



    [Required]
    public bool ReadGroupActionLog { get; set; }

    [Required]
    public bool ReadNoteActionLog { get; set; }



    [Required]
    public bool ReadTagList { get; set; }

    [Required]
    public bool EditTagList { get; set; }

    public GroupRoleModel()
    {

    }

    public GroupRoleModel(int groupId)
    {
        GroupId = groupId;
    }

    public GroupRoleModel(int groupId, GroupRoleDTO dto)
    {
        GroupId = groupId;
        Name = dto.Name;

        GroupEdit = dto.GroupEdit;
        GroupClose = dto.GroupClose;

        ReadNoteList = dto.ReadNoteList;
        ReadNote = dto.ReadNote;
        CreateNote = dto.CreateNote;
        EditNote = dto.EditNote;
        DeleteNote = dto.DeleteNote;

        ReadMemberList = dto.ReadMemberList;
        EditMemberList = dto.EditMemberList;

        ReadRoleList = dto.ReadRoleList;
        EditRoleList = dto.EditRoleList;

        ReadGroupActionLog = dto.ReadGroupActionLog;
        ReadNoteActionLog = dto.ReadNoteActionLog;

        ReadTagList = dto.ReadTagList;
        EditTagList = dto.EditTagList;
    }
}

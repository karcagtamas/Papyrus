using System.ComponentModel.DataAnnotations;
using Papyrus.Shared.Attributes;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Shared.Models.Groups;

public class GroupRoleModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    [LocalizedMaxLength(40, ErrorMessage = "Max length is 40")]
    public string Name { get; set; } = default!;

    [Required]
    public int GroupId { get; set; }


    [Required]
    public bool GroupEdit { get; set; }



    [Required]
    public bool ReadNoteList { get; set; } = true;

    [Required]
    public bool ReadNote { get; set; } = true;

    [Required]
    public bool DeleteNote { get; set; }

    [Required]
    public bool EditNote { get; set; }



    [Required]
    public bool ReadMemberList { get; set; } = true;

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
    public bool ReadTagList { get; set; } = true;

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

        ReadNoteList = dto.ReadNoteList;
        ReadNote = dto.ReadNote;
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

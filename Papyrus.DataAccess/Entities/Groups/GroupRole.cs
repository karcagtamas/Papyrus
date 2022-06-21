using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities.Groups;

public class GroupRole : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public int GroupId { get; set; }

    [Required]
    public bool ReadOnly { get; set; }

    [Required]
    public bool IsDefault { get; set; }




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

    public virtual Group Group { get; set; } = default!;
    public virtual ICollection<GroupMember> Members { get; set; } = default!;
}

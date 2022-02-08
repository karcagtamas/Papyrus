using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;

namespace NoteWeb.DataAccess.Entities;

public class GroupRole : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int GroupId { get; set; }

    [Required]
    public bool Edit { get; set; }

    [Required]
    public bool Close { get; set; }

    [Required]
    public bool ReadNoteList { get; set; }

    [Required]
    public bool CreateNote { get; set; }

    [Required]
    public bool DeleteNote { get; set; }

    [Required]
    public bool ReadNote { get; set; }

    [Required]
    public bool EditNote { get; set; }

    [Required]
    public bool ReadMemberList { get; set; }

    [Required]
    public bool EditMember { get; set; }

    [Required]
    public bool ReadRoleList { get; set; }

    [Required]
    public bool EditRole { get; set; }

    [Required]
    public bool ReadGroupActionLog { get; set; }

    [Required]
    public bool ReadNoteActionLog { get; set; }

    [Required]
    public bool ReadTagList { get; set; }

    [Required]
    public bool EditTag { get; set; }

    [Required]
    public bool ReadOnly { get; set; }

    public virtual Group Group { get; set; }
    public virtual ICollection<GroupMember> Members { get; set; }
}

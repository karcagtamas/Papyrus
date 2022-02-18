using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class GroupActionLog : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int GroupId { get; set; }

    public virtual Group Group { get; set; } = default!;
}

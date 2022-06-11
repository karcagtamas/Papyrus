using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities.Groups;

public class GroupActionLog : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int GroupId { get; set; }

    [Required]
    public DateTime DateTime { get; set; }

    public virtual Group Group { get; set; } = default!;
}

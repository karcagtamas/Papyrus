using KarcagS.Common.Tools.Entities;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities;

public class Language : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string ShortName { get; set; } = default!;

    public virtual ICollection<User> Users { get; set; } = default!;
}

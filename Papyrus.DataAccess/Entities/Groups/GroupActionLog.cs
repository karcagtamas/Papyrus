using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;
using KarcagS.Shared.Common;
using Papyrus.Shared.Enums.Groups;

namespace Papyrus.DataAccess.Entities.Groups;

public class GroupActionLog : IEntity<long>, IIdentified<long>, ICreationEntity
{
    [Key]
    [Required]
    public long Id { get; set; }

    [Required]
    public int GroupId { get; set; }

    [Required]
    public DateTime Creation { get; set; }

    [Required]
    public GroupActionLogType Type { get; set; }

    [Required]
    public string Text { get; set; } = default!;

    public string? PerformerId { get; set; }

    public virtual Group Group { get; set; } = default!;
    public virtual User? Performer { get; set; }
}

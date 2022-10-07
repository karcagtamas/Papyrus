using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Attributes;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class Post : IEntity<int>, ICreationEntity, ILastUpdateEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [User(onlyInit: true)]
    public string? CreatorId { get; set; }

    [Required]
    public DateTime Creation { get; set; }

    [User]
    public string? LastUpdaterId { get; set; }

    [Required]
    public DateTime LastUpdate { get; set; }

    [Required]
    public string Content { get; set; } = default!;

    public virtual User? Creator { get; set; } = default!;
    public virtual User? LastUpdater { get; set; } = default!;
}

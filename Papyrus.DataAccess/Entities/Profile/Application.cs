using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Attributes;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities.Profile;

public class Application : IEntity<string>, ICreationEntity, ILastUpdateEntity
{
    [Key]
    [Required]
    public string Id { get; set; } = default!;

    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    [Required]
    [User(onlyInit: true)]
    public string UserId { get; set; } = default!;

    [Required]
    public string PublicId { get; set; } = default!;

    [Required]
    public string SecretId { get; set; } = default!;

    [Required]
    public DateTime Creation { get; set; }

    [Required]
    public DateTime LastUpdate { get; set; }

    public virtual User User { get; set; } = default!;
}

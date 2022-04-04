using Karcags.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess.Entities;

namespace Papyrus.DataAccess;

public class NoteWebContext : IdentityDbContext<User, Role, string>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupActionLog> GroupActionLogs { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupRole> GroupRoles { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<NoteActionLog> NoteActionLogs { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public NoteWebContext(DbContextOptions<NoteWebContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasIndex(user => user.UserName)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        // Refresh token
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(t => t.Token)
            .IsUnique();
        modelBuilder.Entity<RefreshToken>()
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // Group
        modelBuilder.Entity<Group>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.CreatedGroups)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // Group Action Log
        modelBuilder.Entity<GroupActionLog>()
            .HasOne(x => x.Group)
            .WithMany(x => x.ActionLogs)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // Group Member
        modelBuilder.Entity<GroupMember>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Members)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
        modelBuilder.Entity<GroupMember>()
            .HasOne(x => x.User)
            .WithMany(x => x.Groups)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
        modelBuilder.Entity<GroupMember>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Members)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // Group Role
        modelBuilder.Entity<GroupRole>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Roles)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // Note
        modelBuilder.Entity<Note>()
            .HasOne(x => x.User)
            .WithMany(x => x.Notes)
            .OnDelete(DeleteBehavior.ClientCascade);
        modelBuilder.Entity<Note>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Notes)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Note Action Log
        modelBuilder.Entity<NoteActionLog>()
            .HasOne(x => x.Note)
            .WithMany(x => x.ActionLogs)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // Tag
        modelBuilder.Entity<Tag>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Tags)
            .OnDelete(DeleteBehavior.ClientCascade);
        modelBuilder.Entity<Tag>()
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .OnDelete(DeleteBehavior.ClientCascade);
    }

    public override int SaveChanges()
    {
        var dateEntries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ILastUpdateEntity && e.State is EntityState.Added or EntityState.Modified);

        var now = DateTime.Now;

        foreach (var entry in dateEntries)
        {
            ((ILastUpdateEntity)entry.Entity).LastUpdate = now;

            /*if (entityEntry.State == EntityState.Added)
            {
                ((IEntity<object>)entityEntry.Entity).Creation = now;
            }*/
        }

        return base.SaveChanges();
    }
}

using Karcags.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteWeb.DataAccess.Entities;

namespace NoteWeb.DataAccess;

public class NoteWebContext : IdentityDbContext<User, Role, string>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

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

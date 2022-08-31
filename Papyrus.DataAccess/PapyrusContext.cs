using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Editor;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;

namespace Papyrus.DataAccess;

public class PapyrusContext : IdentityDbContext<User, Role, string>
{
    public DbSet<Language> Languages { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupActionLog> GroupActionLogs { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupRole> GroupRoles { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<NoteActionLog> NoteActionLogs { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NoteTag> NoteTags { get; set; }

    public DbSet<EditorMember> EditorMembers { get; set; }

    public PapyrusContext(DbContextOptions<PapyrusContext> options) : base(options)
    {
        Languages = default!;
        RefreshTokens = default!;
        Groups = default!;
        GroupActionLogs = default!;
        GroupMembers = default!;
        GroupRoles = default!;
        Notes = default!;
        NoteActionLogs = default!;
        Tags = default!;
        NoteTags = default!;
        EditorMembers = default!;
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
        modelBuilder.Entity<User>()
            .HasOne(x => x.Language)
            .WithMany(x => x.Users)
            .OnDelete(DeleteBehavior.SetNull);

        // Language
        modelBuilder.Entity<Language>()
            .HasIndex(x => x.ShortName)
            .IsUnique();
        modelBuilder.Entity<Language>()
            .HasData(new List<Language> { new() { Id = 1, Name = "English", ShortName = "en-US" }, new() { Id = 2, Name = "Hungarian", ShortName = "hu-HU" } });

        // Refresh token
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(t => t.Token)
            .IsUnique();
        modelBuilder.Entity<RefreshToken>()
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Group
        modelBuilder.Entity<Group>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.CreatedGroups)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Group Action Log
        modelBuilder.Entity<GroupActionLog>()
            .HasOne(x => x.Group)
            .WithMany(x => x.ActionLogs)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<GroupActionLog>()
            .HasOne(x => x.Performer)
            .WithMany(x => x.PerformedGroupActionLogs)
            .OnDelete(DeleteBehavior.SetNull);

        // Group Member
        modelBuilder.Entity<GroupMember>()
            .HasAlternateKey(x => new { x.GroupId, x.UserId });
        modelBuilder.Entity<GroupMember>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Members)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<GroupMember>()
            .HasOne(x => x.User)
            .WithMany(x => x.Groups)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<GroupMember>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Members)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<GroupMember>()
            .HasOne(x => x.AddedBy)
            .WithMany(x => x.AddedGroupMembers)
            .OnDelete(DeleteBehavior.SetNull);

        // Group Role
        modelBuilder.Entity<GroupRole>()
            .HasAlternateKey(x => new { x.GroupId, x.Name });
        modelBuilder.Entity<GroupRole>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Roles)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Note
        modelBuilder.Entity<Note>()
            .HasOne(x => x.User)
            .WithMany(x => x.Notes)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Note>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Notes)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Note>()
            .HasOne(x => x.Creator)
            .WithMany(x => x.CreatedNotes)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Note>()
            .HasOne(x => x.LastUpdater)
            .WithMany(x => x.LastUpdatedNotes)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Note>()
            .HasCheckConstraint("CK_Note_Owner", "(GroupId IS NOT NULL OR UserId IS NOT NULL) AND NOT (GroupId IS NOT NULL AND UserId IS NOT NULL)");

        // Note Action Log
        modelBuilder.Entity<NoteActionLog>()
            .HasOne(x => x.Note)
            .WithMany(x => x.ActionLogs)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<NoteActionLog>()
            .HasOne(x => x.Performer)
            .WithMany(x => x.PerformedNoteActionLogs)
            .OnDelete(DeleteBehavior.SetNull);

        // Tag
        modelBuilder.Entity<Tag>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Tags)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Tag>()
            .HasOne(x => x.User)
            .WithMany(x => x.Tags)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Tag>()
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Tag>()
            .HasCheckConstraint("CK_Tag_Owner", "(GroupId IS NOT NULL OR UserId IS NOT NULL) AND NOT (GroupId IS NOT NULL AND UserId IS NOT NULL)");

        // Note Tags
        modelBuilder.Entity<NoteTag>()
            .HasKey(x => new { x.NoteId, x.TagId });
        modelBuilder.Entity<NoteTag>()
            .HasOne(x => x.Note)
            .WithMany(x => x.Tags)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<NoteTag>()
            .HasOne(x => x.Tag)
            .WithMany(x => x.Notes)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Editor Member
        modelBuilder.Entity<EditorMember>()
            .HasAlternateKey(x => new { x.UserId, x.NoteId, x.ConnectionId });
        modelBuilder.Entity<EditorMember>()
            .HasOne(x => x.User)
            .WithMany(x => x.EditorMemberships)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<EditorMember>()
            .HasOne(x => x.Note)
            .WithMany(x => x.EditorMemberships)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

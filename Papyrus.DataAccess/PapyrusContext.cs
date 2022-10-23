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
    public DbSet<Translation> Translations { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<AppAccess> AppAccesses { get; set; }

    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupRole> GroupRoles { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NoteTag> NoteTags { get; set; }
    public DbSet<NoteAccess> NoteAccesses { get; set; }

    public DbSet<ActionLog> ActionLogs { get; set; }

    public DbSet<EditorMember> EditorMembers { get; set; }

    public PapyrusContext(DbContextOptions<PapyrusContext> options) : base(options)
    {
        Languages = default!;
        Translations = default!;
        RefreshTokens = default!;
        Posts = default!;
        AppAccesses = default!;
        Groups = default!;
        GroupMembers = default!;
        GroupRoles = default!;
        Folders = default!;
        Notes = default!;
        Tags = default!;
        NoteTags = default!;
        NoteAccesses = default!;
        ActionLogs = default!;
        EditorMembers = default!;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .RegisterCommonEntities()
            .RegisterGroupEntities()
            .RegisterNoteEntities()
            .RegisterTempEntities();
    }
}

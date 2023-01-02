using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Editor;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.DataAccess.Entities.Profile;

namespace Papyrus.DataAccess;

public class PapyrusContext : IdentityDbContext<User, Role, string>
{
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Translation> Translations => Set<Translation>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<AppAccess> AppAccesses => Set<AppAccess>();

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<GroupRole> GroupRoles => Set<GroupRole>();
    public DbSet<Folder> Folders => Set<Folder>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<NoteTag> NoteTags => Set<NoteTag>();
    public DbSet<NoteAccess> NoteAccesses => Set<NoteAccess>();

    public DbSet<ActionLog> ActionLogs => Set<ActionLog>();
    public DbSet<Application> Applications => Set<Application>();

    public DbSet<EditorMember> EditorMembers => Set<EditorMember>();

    public PapyrusContext(DbContextOptions<PapyrusContext> options) : base(options)
    {
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

using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Entities.Editor;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.Enums.Groups;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.DataAccess;

public static class ContextExtensions
{
    public static ModelBuilder RegisterCommonEntities(this ModelBuilder builder)
    {
        // User
        builder.Entity<User>(b =>
        {
            b.HasIndex(user => user.UserName)
                .IsUnique();
            b.HasIndex(user => user.Email)
                .IsUnique();
            b.HasOne(x => x.Language)
                .WithMany(x => x.Users)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        });

        // User Roles
        builder.Entity<Role>(b =>
        {
            b.HasMany(e => e.Users)
                .WithOne()
                .HasForeignKey(x => x.RoleId)
                .IsRequired();
        });

        // Language
        builder.Entity<Language>()
            .HasIndex(x => x.ShortName)
            .IsUnique();
        builder.Entity<Language>()
            .HasData(new List<Language>
            {
                new() { Id = 1, Name = "English", ShortName = "en-US" },
                new() { Id = 2, Name = "Hungarian", ShortName = "hu-HU" }
            });

        // Translation
        builder.Entity<Translation>()
            .HasKey(x => new { x.Key, x.Segment, x.Language });
        builder.Entity<Translation>()
            .HasData(new List<Translation>
            {
                new() { Key = "Administrator", Segment = "Role", Language = "en-US", Value = "Administrator" },
                new() { Key = "Administrator", Segment = "Role", Language = "hu-HU", Value = "Adminisztrátor" },
                new() { Key = "Moderator", Segment = "Role", Language = "en-US", Value = "Moderator" },
                new() { Key = "Moderator", Segment = "Role", Language = "hu-HU", Value = "Moderátor" },
                new() { Key = "User", Segment = "Role", Language = "en-US", Value = "User" },
                new() { Key = "User", Segment = "Role", Language = "hu-HU", Value = "Felhasználó" },
                new() { Key = "English", Segment = "Language", Language = "en-US", Value = "English" },
                new() { Key = "English", Segment = "Language", Language = "hu-HU", Value = "Angol" },
                new() { Key = "Hungarian", Segment = "Language", Language = "en-US", Value = "Hungarian" },
                new() { Key = "Hungarian", Segment = "Language", Language = "hu-HU", Value = "Magyar" },
                new() { Key = "Administrator", Segment = "GroupRole", Language = "en-US", Value = "Administrator" },
                new() { Key = "Administrator", Segment = "GroupRole", Language = "hu-HU", Value = "Adminisztrátor" },
                new() { Key = "Moderator", Segment = "GroupRole", Language = "en-US", Value = "Moderator" },
                new() { Key = "Moderator", Segment = "GroupRole", Language = "hu-HU", Value = "Moderátor" },
                new() { Key = "Default", Segment = "GroupRole", Language = "en-US", Value = "Default" },
                new() { Key = "Default", Segment = "GroupRole", Language = "hu-HU", Value = "Alapértelmezett" }
            });
        builder.Entity<Translation>()
            .HasData(new List<Translation>
            {
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.Create), Segment = "Group", Language = "en-US", Value = "Created" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.Create), Segment = "Group", Language = "hu-HU", Value = "Csoport létrehozva" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.Close), Segment = "Group", Language = "en-US", Value = "Closed" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.Close), Segment = "Group", Language = "hu-HU", Value = "Csoport lezárva" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.Open), Segment = "Group", Language = "en-US", Value = "Opened" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.Open), Segment = "Group", Language = "hu-HU", Value = "Csoport kinyitva" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.RoleCreate), Segment = "Group", Language = "en-US", Value = "Role is created" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.RoleCreate), Segment = "Group", Language = "hu-HU", Value = "Szerepkör létrehozva" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.RoleEdit), Segment = "Group", Language = "en-US", Value = "Role is edited" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.RoleEdit), Segment = "Group", Language = "hu-HU", Value = "Szerepkör szerkesztve" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.RoleRemove), Segment = "Group", Language = "en-US", Value = "Role is removed" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.RoleRemove), Segment = "Group", Language = "hu-HU", Value = "Szerepkör törölve" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.MemberAdd), Segment = "Group", Language = "en-US", Value = "Member is added" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.MemberAdd), Segment = "Group", Language = "hu-HU", Value = "Tag hozzáadva" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.MemberEdit), Segment = "Group", Language = "en-US", Value = "Member is edited" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.MemberEdit), Segment = "Group", Language = "hu-HU", Value = "Tag szerkesztve" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.MemberRemove), Segment = "Group", Language = "en-US", Value = "Member is removed" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.MemberRemove), Segment = "Group", Language = "hu-HU", Value = "Tag törölve" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.TagCreate), Segment = "Group", Language = "en-US", Value = "Tag is created" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.TagCreate), Segment = "Group", Language = "hu-HU", Value = "Címke létrehozva" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.TagEdit), Segment = "Group", Language = "en-US", Value = "Tag is edited" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.TagEdit), Segment = "Group", Language = "hu-HU", Value = "Címke szerkesztve" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.TagRemove), Segment = "Group", Language = "en-US", Value = "Tag is removed" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.TagRemove), Segment = "Group", Language = "hu-HU", Value = "Címke törölve" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.DataEdit), Segment = "Group", Language = "en-US", Value = "Data is edited" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.DataEdit), Segment = "Group", Language = "hu-HU", Value = "Adatok szerkesztve" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.NoteCreate), Segment = "Group", Language = "en-US", Value = "Note is created" },
                new() { Key = GroupActionLogTypeConverter.GetLogKey(GroupActionLogType.NoteCreate), Segment = "Group", Language = "hu-HU", Value = "Jegyzet létrehozva" },
            });
        builder.Entity<Translation>()
            .HasData(new List<Translation>
            {
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Create), Segment = "Note", Language = "en-US", Value = "Created" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Create), Segment = "Note", Language = "hu-HU", Value = "Létrehozva" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.TitleEdit), Segment = "Note", Language = "en-US", Value = "Title is edited" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.TitleEdit), Segment = "Note", Language = "hu-HU", Value = "Cím szerkesztve" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.ContentEdit), Segment = "Note", Language = "en-US", Value = "Content is edited" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.ContentEdit), Segment = "Note", Language = "hu-HU", Value = "Tartalom szerkesztve" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.TagEdit), Segment = "Note", Language = "en-US", Value = "Tag(s) added or removed" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.TagEdit), Segment = "Note", Language = "hu-HU", Value = "Címke/Címkék hozzáadva vagy törölve" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Delete), Segment = "Note", Language = "en-US", Value = "Deleted" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Delete), Segment = "Note", Language = "hu-HU", Value = "Törölve" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Publish), Segment = "Note", Language = "en-US", Value = "Public status is changed" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Publish), Segment = "Note", Language = "hu-HU", Value = "Nyílvános státusz megváltoztatva" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Archived), Segment = "Note", Language = "en-US", Value = "Archived status is changed" },
                new() { Key = NoteActionLogTypeConverter.GetLogKey(NoteActionLogType.Archived), Segment = "Note", Language = "hu-HU", Value = "Archivált státusz megváltoztatva" },
            });
        builder.Entity<Translation>()
            .HasData(new List<Translation>
            {
                new() { Key = "0", Segment = "Theme", Language = "en-US", Value = "Light Theme" },
                new() { Key = "0", Segment = "Theme", Language = "hu-HU", Value = "Világos Téma" },
                new() { Key = "1", Segment = "Theme", Language = "en-US", Value = "Dark Theme" },
                new() { Key = "1", Segment = "Theme", Language = "hu-HU", Value = "Sötét Téma" },
            });

        // Refresh token
        builder.Entity<RefreshToken>()
            .HasIndex(t => t.Token)
            .IsUnique();
        builder.Entity<RefreshToken>()
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Action Logs
        builder.Entity<ActionLog>()
            .HasOne(x => x.Performer)
            .WithMany(x => x.ActionLogs)
            .OnDelete(DeleteBehavior.SetNull);

        return builder;
    }

    public static ModelBuilder RegisterGroupEntities(this ModelBuilder builder)
    {
        // Group
        builder.Entity<Group>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.CreatedGroups)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Group Member
        builder.Entity<GroupMember>()
            .HasAlternateKey(x => new { x.GroupId, x.UserId });
        builder.Entity<GroupMember>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Members)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<GroupMember>()
            .HasOne(x => x.User)
            .WithMany(x => x.Groups)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<GroupMember>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Members)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<GroupMember>()
            .HasOne(x => x.AddedBy)
            .WithMany(x => x.AddedGroupMembers)
            .OnDelete(DeleteBehavior.SetNull);

        // Group Role
        builder.Entity<GroupRole>()
            .HasAlternateKey(x => new { x.GroupId, x.Name });
        builder.Entity<GroupRole>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Roles)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }

    public static ModelBuilder RegisterNoteEntities(this ModelBuilder builder)
    {
        // Note
        builder.Entity<Note>()
            .HasOne(x => x.User)
            .WithMany(x => x.Notes)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Note>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Notes)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Note>()
            .HasOne(x => x.Creator)
            .WithMany(x => x.CreatedNotes)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        builder.Entity<Note>()
            .HasOne(x => x.LastUpdater)
            .WithMany(x => x.LastUpdatedNotes)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        builder.Entity<Note>()
            .HasCheckConstraint("CK_Note_Owner", "(GroupId IS NOT NULL OR UserId IS NOT NULL) AND NOT (GroupId IS NOT NULL AND UserId IS NOT NULL)");

        // Tag
        builder.Entity<Tag>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Tags)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Tag>()
            .HasOne(x => x.User)
            .WithMany(x => x.Tags)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Tag>()
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Tag>()
            .HasCheckConstraint("CK_Tag_Owner", "(GroupId IS NOT NULL OR UserId IS NOT NULL) AND NOT (GroupId IS NOT NULL AND UserId IS NOT NULL)");

        // Note Tags
        builder.Entity<NoteTag>()
            .HasKey(x => new { x.NoteId, x.TagId });
        builder.Entity<NoteTag>()
            .HasOne(x => x.Note)
            .WithMany(x => x.Tags)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<NoteTag>()
            .HasOne(x => x.Tag)
            .WithMany(x => x.Notes)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }

    public static ModelBuilder RegisterTempEntities(this ModelBuilder builder)
    {
        // Editor Member
        builder.Entity<EditorMember>()
            .HasAlternateKey(x => new { x.UserId, x.NoteId, x.ConnectionId });
        builder.Entity<EditorMember>()
            .HasOne(x => x.User)
            .WithMany(x => x.EditorMemberships)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<EditorMember>()
            .HasOne(x => x.Note)
            .WithMany(x => x.EditorMemberships)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}

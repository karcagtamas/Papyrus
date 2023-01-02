using KarcagS.Blazor.Common.Components.Menu;
using KarcagS.Blazor.Common.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Client.Shared;

public static class PapyrusMenu
{
    public static List<MenuItem> BuildMenuItems(IHelperService helper, AuthenticationState state)
    {
        var items = new List<MenuItem>();

        items.AddRange(new List<MenuItem>()
        {
            MenuItem.CreateItem("Home", "home")
                .AddResourceKey("Home")
                .AddIcon(Icons.Material.Filled.Home)
                .AddIconColor(Color.Tertiary)
                .IsAuthenticated(false),
            MenuItem.CreateItem("Dashboard", "dashboard")
                .AddResourceKey("Dashboard")
                .AddIcon(Icons.Material.Filled.Dashboard),
            MenuItem.CreateItem("Search", "search")
                .AddResourceKey("Search")
                .AddIcon(Icons.Material.Filled.Search)
                .IsAuthenticated(false),
            MenuItem.CreateGroupItem("Profile")
                .AddResourceKey("Profile")
                .AddIcon(Icons.Material.Filled.Person)
                .AddIconColor(Color.Success)
                .AddItem(MenuItem.CreateItem("Data", "profile/my").AddResourceKey("Data").AddIcon(Icons.Material.Filled.DataArray))
                .AddItem(MenuItem.CreateItem("Applications", "profile/apps").AddResourceKey("Applications").AddIcon(Icons.Material.Filled.SettingsApplications)),
            MenuItem.CreateItem("My Groups", "my-groups")
                .AddResourceKey("MyGroups")
                .AddIcon(Icons.Material.Filled.Groups),
            MenuItem.CreateGroupItem("Notes")
                .AddResourceKey("Notes")
                .AddIcon(Icons.Material.Filled.Notes)
                .AddItem(MenuItem.CreateItem("List", "notes").AddResourceKey("NoteList").AddIcon(Icons.Material.Filled.NoteAlt))
                .AddItem(MenuItem.CreateItem("Tags", "notes/tags").AddResourceKey("NoteTags").AddIcon(Icons.Material.Filled.Tag)),
        });

        if (helper.IsInRole(state, "Administrator", "Moderator"))
        {
            items.Add(
                MenuItem.CreateGroupItem("Administration")
                    .AddResourceKey("Administration")
                    .AddIcon(Icons.Material.Filled.AdminPanelSettings)
                    .AddIconColor(Color.Warning)
                    .AddItem(MenuItem.CreateItem("Users", "admin/users").AddResourceKey("Users").AddIcon(Icons.Material.Filled.ManageAccounts))
            );
        }

        items.Add(
            MenuItem.CreateItem("About", "about")
                .AddResourceKey("About")
                .AddIcon(Icons.Material.Filled.Web)
                .IsAuthenticated(false)
        );

        return items;
    }

    public static List<MenuItem> BuildGroupItems(int groupId, GroupPageRightsDTO rights)
    {
        var items = new List<MenuItem>
        {
            MenuItem.CreateItem("Data", $"groups/{groupId}")
                .AddResourceKey("GroupData")
                .AddIcon(Icons.Material.Filled.Info),
        };

        var noteItem = MenuItem.CreateGroupItem("Notes")
                .AddResourceKey("GroupNotes")
                .AddIcon(Icons.Material.Filled.Notes);

        if (rights.NotePageEnabled)
        {
            noteItem = noteItem.AddItem(MenuItem.CreateItem("List", $"groups/{groupId}/notes").AddResourceKey("GroupNoteList").AddIcon(Icons.Material.Filled.NoteAlt));
        }

        if (rights.TagPageEnabled)
        {
            noteItem = noteItem.AddItem(MenuItem.CreateItem("Tags", $"groups/{groupId}/notes/tags").AddResourceKey("GroupNoteTags").AddIcon(Icons.Material.Filled.Tag));
        }

        if (rights.NotePageEnabled || rights.TagPageEnabled)
        {
            items.Add(noteItem);
        }

        if (rights.MemberPageEnabled)
        {
            items.Add(MenuItem.CreateItem("Members", $"groups/{groupId}/members")
                .AddResourceKey("GroupMembers")
                .AddIcon(Icons.Material.Filled.People));
        }

        if (rights.RolePageEnabled)
        {
            items.Add(MenuItem.CreateItem("Roles", $"groups/{groupId}/roles")
                .AddResourceKey("GroupRoles")
                .AddIcon(Icons.Material.Filled.SettingsSuggest));
        }

        if (rights.LogPageEnabled)
        {
            items.Add(MenuItem.CreateItem("Logs", $"groups/{groupId}/logs")
                .AddResourceKey("GroupLogs")
                .AddIcon(Icons.Material.Filled.History)
                .AddIconColor(Color.Warning));
        }

        return items;
    }
}

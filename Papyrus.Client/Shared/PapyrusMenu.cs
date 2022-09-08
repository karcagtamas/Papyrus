using KarcagS.Blazor.Common.Components.Menu;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Papyrus.Client.Services.Interfaces;

namespace Papyrus.Client.Shared;

public static class PapyrusMenu
{
    public static List<MenuItem> BuildMenuItems(ICommonService commonService, AuthenticationState state)
    {
        var items = new List<MenuItem>();

        items.AddRange(new List<MenuItem>()
        {
            MenuItem.CreateItem("Dashboard", "dashboard")
                .AddResourceKey("Dashboard")
                .AddIcon(Icons.Filled.Dashboard),
            MenuItem.CreateItem("Search", "search")
                .AddResourceKey("Search")
                .AddIcon(Icons.Filled.Search),
            MenuItem.CreateItem("Profile", "my")
                .AddResourceKey("Profile")
                .AddIcon(Icons.Filled.Person),
            MenuItem.CreateItem("My Groups", "my-groups")
                .AddResourceKey("MyGroups")
                .AddIcon(Icons.Filled.Groups),
            MenuItem.CreateGroupItem("Notes")
                .AddResourceKey("Notes")
                .AddIcon(Icons.Filled.Notes)
                .AddItem(MenuItem.CreateItem("List", "notes").AddResourceKey("NoteList").AddIcon(Icons.Filled.NoteAlt))
                .AddItem(MenuItem.CreateItem("Tags", "notes/tags").AddResourceKey("NoteTags").AddIcon(Icons.Filled.Tag)),
        });

        if (commonService.IsInRole(state, "Administrator", "Moderator"))
        {
            items.Add(
                MenuItem.CreateGroupItem("Administration")
                    .AddResourceKey("Administration")
                    .AddIcon(Icons.Filled.AdminPanelSettings)
                    .AddIconColor(Color.Warning)
                    .AddItem(MenuItem.CreateItem("Users", "users").AddResourceKey("Users").AddIcon(Icons.Filled.ManageAccounts))
            );
        }

        items.Add(
            MenuItem.CreateItem("About", "about")
                .AddResourceKey("About")
                .AddIcon(Icons.Filled.Web)
                .IsAuthenticated(false)
        );

        return items;
    }

    public static List<MenuItem> BuildGroupItems(int groupId)
    {
        return new()
        {
            MenuItem.CreateItem("Data", $"groups/{groupId}")
                .AddResourceKey("GroupData")
                .AddIcon(Icons.Filled.Info),
            MenuItem.CreateGroupItem("Notes")
                .AddResourceKey("GroupNotes")
                .AddIcon(Icons.Filled.Notes)
                .AddItem(MenuItem.CreateItem("List", $"groups/{groupId}/notes").AddResourceKey("GroupNoteList").AddIcon(Icons.Filled.NoteAlt))
                .AddItem(MenuItem.CreateItem("Tags", $"groups/{groupId}/notes/tags").AddResourceKey("GroupNoteTags").AddIcon(Icons.Filled.Tag)),
            MenuItem.CreateItem("Members", $"groups/{groupId}/members")
                .AddResourceKey("GroupMembers")
                .AddIcon(Icons.Filled.People),
            MenuItem.CreateItem("Roles", $"groups/{groupId}/roles")
                .AddResourceKey("GroupRoles")
                .AddIcon(Icons.Filled.SettingsSuggest),
            MenuItem.CreateItem("Logs", $"groups/{groupId}/logs")
                .AddResourceKey("GroupLogs")
                .AddIcon(Icons.Filled.History)
                .AddIconColor(Color.Warning)
        };
    }
}

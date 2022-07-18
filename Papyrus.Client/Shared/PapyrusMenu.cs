using KarcagS.Blazor.Common.Components.Menu;
using MudBlazor;

namespace Papyrus.Client.Shared;

public static class PapyrusMenu
{
    public static readonly List<MenuItem> Items = new()
    {
        MenuItem.CreateItem("Dashboard", "dashboard")
            .AddIcon(Icons.Filled.Dashboard),
        MenuItem.CreateItem("Search", "search")
            .AddIcon(Icons.Filled.Search),
        MenuItem.CreateItem("Profile", "my")
            .AddIcon(Icons.Filled.Person),
        MenuItem.CreateItem("My Groups", "my-groups")
            .AddIcon(Icons.Filled.Groups),
        MenuItem.CreateGroupItem("Notes")
            .AddIcon(Icons.Filled.Notes)
            .AddItem(MenuItem.CreateItem("List", "notes").AddIcon(Icons.Filled.NoteAlt))
            .AddItem(MenuItem.CreateItem("Tags", "notes/tags").AddIcon(Icons.Filled.Tag))
            .AddItem(MenuItem.CreateItem("Logs", "notes/logs").AddIcon(Icons.Filled.History).AddIconColor(Color.Warning)),
        MenuItem.CreateGroupItem("Admin")
            .AddIcon(Icons.Filled.AdminPanelSettings)
            .AddIconColor(Color.Warning)
            .AddItem(MenuItem.CreateItem("Users", "users").AddIcon(Icons.Filled.ManageAccounts))
            .AddItem(MenuItem.CreateItem("Roles", "roles").AddIcon(Icons.Filled.SettingsSuggest)),
        MenuItem.CreateItem("About", "about")
            .AddIcon(Icons.Filled.Web)
            .IsAuthenticated(false)
    };

    public static List<MenuItem> BuildGroupItems(int groupId)
    {
        return new()
        {
            MenuItem.CreateItem("Data", $"groups/{groupId}")
                .AddIcon(Icons.Filled.Info),
            MenuItem.CreateGroupItem("Notes")
                .AddIcon(Icons.Filled.Notes)
                .AddItem(MenuItem.CreateItem("List", $"groups/{groupId}/notes").AddIcon(Icons.Filled.NoteAlt))
                .AddItem(MenuItem.CreateItem("Tags", $"groups/{groupId}/notes/tags").AddIcon(Icons.Filled.Tag))
                .AddItem(MenuItem.CreateItem("Logs", $"groups/{groupId}/notes/logs").AddIcon(Icons.Filled.History).AddIconColor(Color.Warning)),
            MenuItem.CreateItem("Members", $"groups/{groupId}/members")
                .AddIcon(Icons.Filled.People),
            MenuItem.CreateItem("Roles", $"groups/{groupId}/roles")
                .AddIcon(Icons.Filled.SettingsSuggest),
            MenuItem.CreateItem("Logs", $"groups/{groupId}/logs")
                .AddIcon(Icons.Filled.History)
                .AddIconColor(Color.Warning)
        };
    }
}

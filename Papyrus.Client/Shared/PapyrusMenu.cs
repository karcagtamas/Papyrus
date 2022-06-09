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
            .AddItem(MenuItem.CreateItem("Tags", "tags").AddIcon(Icons.Filled.Tag)),
        MenuItem.CreateGroupItem("Admin")
            .AddIcon(Icons.Filled.AdminPanelSettings)
            .AddIconColor(Color.Warning)
            .AddItem(MenuItem.CreateItem("Users", "users").AddIcon(Icons.Filled.ManageAccounts))
            .AddItem(MenuItem.CreateItem("Roles", "roles").AddIcon(Icons.Filled.SettingsSuggest)),
        MenuItem.CreateItem("About", "about")
            .AddIcon(Icons.Filled.Web)
            .IsAuthenticated(false)
    };

    public class MenuItem
    {
        public string Title { get; set; }
        public string? Icon { get; set; }
        public Color? IconColor { get; set; }
        public string? Path { get; set; }
        public List<MenuItem> Children { get; set; }
        public bool IsDivider { get; set; } = false;
        public Action? Action { get; set; }
        public bool Authenticated { get; set; } = true;

        public bool IsGroup { get { return Children.Count > 0; } }
        public bool IsAction { get { return Path != null; } }

        public MenuItem(string title, string? icon, string? path, List<MenuItem> children)
        {
            Title = title;
            Icon = icon;
            Path = path;
            Children = children;
        }

        public static MenuItem CreateGroupItem(string title, List<MenuItem> children) 
        {
            return new(title, null, null, children);
        }

        public static MenuItem CreateGroupItem(string title)
        {
            return new(title, null, null, new List<MenuItem>());
        }

        public static MenuItem CreateItem(string title, string path)
        {
            return new(title, null, path, new List<MenuItem>());
        }

        public static MenuItem CreateActionItem(string title, Action action)
        {
            MenuItem item = new(title, null, null, new List<MenuItem>());
            item.Action = action;
            return item;
        }

        public static MenuItem CreateDivider()
        {
            MenuItem item = new("", null, null, new List<MenuItem>());
            item.IsDivider = true;
            return item;
        }

        public MenuItem AddItem(MenuItem item) 
        {
            Children.Add(item);
            return this;
        }

        public MenuItem AddIcon(string icon)
        {
            Icon = icon;
            return this;
        }

        public MenuItem AddIconColor(Color color)
        {
            IconColor = color;
            return this;
        }

        public MenuItem IsAuthenticated(bool authenticated)
        {
            Authenticated = authenticated;
            return this;
        }
    }
}

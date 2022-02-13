using MudBlazor;

namespace Papyrus.Client.Shared;

public static class PapyrusTheme
{
    private static readonly MudTheme Theme = new()
    {
        Palette = new Palette
        {
            Primary = Colors.Pink.Darken4,
            Secondary = Colors.Teal.Darken4,
            Tertiary = Colors.Indigo.Darken4,
            Info = Colors.Blue.Darken4,
            Success = Colors.Green.Darken4,
            Warning = Colors.Orange.Darken4,
            Error = Colors.Red.Darken4,
            Divider = Colors.Pink.Darken4,
            DrawerBackground = Colors.Pink.Lighten5
        },
        LayoutProperties = new LayoutProperties
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "300px"
        },
        Typography = new Typography
        {
            Default = new Default { FontFamily = new[] { "Bree Serif", "serif" } }
        }
    };

    public static MudTheme GetTheme()
    {
        return Theme;
    }
}

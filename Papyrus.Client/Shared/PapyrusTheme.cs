using MudBlazor;
using MudBlazor.Utilities;

namespace Papyrus.Client.Shared;

public static class PapyrusTheme
{
    public static readonly string Main = "#0D2971";
    private static readonly MudColor MainColor = new(Main);
    private static readonly MudColor SecondaryColor = new("#581043");
    private static readonly MudColor TertiaryColor = new("#361B3A");
    private static readonly MudColor WarningColor = new("#C07C38");
    private static readonly MudColor ErrorColor = new("#850525");
    private static readonly MudColor InfoColor = new("#66ADA1");
    private static readonly MudColor SuccessColor = new("#40921A");
    private static readonly MudColor White = new("#FFFFFF");
    

    private static readonly MudTheme Theme = new()
    {
        Palette = new Palette
        {
            Primary = MainColor,
            Secondary = SecondaryColor,
            Tertiary = TertiaryColor,
            Info = InfoColor,
            Success = SuccessColor,
            Warning = WarningColor,
            Error = ErrorColor,
            Divider = MainColor,
            DrawerBackground = MainColor.ColorLighten(.71),
            DrawerText = MainColor,
            DrawerIcon = MainColor,
            ActionDefault = MainColor,
            AppbarText = White
        },
        LayoutProperties = new LayoutProperties
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "300px"
        },
        Typography = new Typography
        {
            Default = new Default { FontFamily = new[] { "Bree Serif", "serif" } },
            Button = new Button
            {
                TextTransform = "none"
            }
        }
    };

    public static MudTheme GetTheme()
    {
        return Theme;
    }
}

using KarcagS.Blazor.Common.Theme;
using MudBlazor;
using MudBlazor.Utilities;

namespace Papyrus.Client.Shared;

public static class PapyrusTheme
{
    public static readonly AppTheme AppTheme = new(
        new AppTheme.AppColorPalette
        {
            MainColor = "#0D2971",
            SecondaryColor = "#581043",
            TertiaryColor = "#361B3A",
            WarningColor = "#C07C38",
            ErrorColor = "#850525",
            InfoColor = "#66ADA1",
            SuccessColor = "#40921A",
            White = "#FFFFFF",
            DrawerBackground = new MudColor("#0D2971").ColorLighten(0.71)
        },
        new AppTheme.AppColorPalette
        {
            MainColor = "#537deb",
            SecondaryColor = "#e053b7",
            TertiaryColor = "#b16ebb",
            WarningColor = "#e1bd9a",
            ErrorColor = "#f84c77",
            InfoColor = "#b2d6d0",
            SuccessColor = "#95e570",
            White = "#FFFFFF",
            DrawerBackground = new MudColor("#32333d").ColorDarken(0.1)
        });

    public static MudTheme GetTheme() => AppTheme.Theme;
}

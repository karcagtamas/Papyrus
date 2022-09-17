using KarcagS.Blazor.Common.Theme;
using MudBlazor;

namespace Papyrus.Client.Shared;

public static class PapyrusTheme
{
    public static readonly AppTheme AppTheme = new(
        new AppTheme.AppColorPalette
        {
            MainColor = new("#0D2971"),
            SecondaryColor = new("#581043"),
            TertiaryColor = new("#361B3A"),
            WarningColor = new("#C07C38"),
            ErrorColor = new("#850525"),
            InfoColor = new("#66ADA1"),
            SuccessColor = new("#40921A"),
            White = new("#FFFFFF"),
        },
        new AppTheme.AppColorPalette
        {
            MainColor = new("#537deb"),
            SecondaryColor = new("#e053b7"),
            TertiaryColor = new("#b16ebb"),
            WarningColor = new("#e1bd9a"),
            ErrorColor = new("#f84c77"),
            InfoColor = new("#b2d6d0"),
            SuccessColor = new("#95e570"),
            White = new("#FFFFFF"),
        });

    public static MudTheme GetTheme() => AppTheme.Theme;
}

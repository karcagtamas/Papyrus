using KarcagS.Blazor.Common.Theme;
using MudBlazor;
using MudBlazor.Utilities;

namespace Papyrus.Client.Shared;

public static class PapyrusTheme
{
    public static readonly AppTheme AppTheme = new AppTheme(new AppTheme.AppColorPalette
    {
        MainColor = new("#0D2971"),
        SecondaryColor = new("#581043"),
        TertiaryColor = new("#361B3A"),
        WarningColor = new("#C07C38"),
        ErrorColor = new("#850525"),
        InfoColor = new("#66ADA1"),
        SuccessColor = new("#40921A"),
        White = new("#FFFFFF")
    });

    public static MudTheme GetTheme() => AppTheme.Theme;
}

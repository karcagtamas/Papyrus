namespace Papyrus.Shared.Helpers;

/// <summary>
/// Display helper
/// </summary>
public static class WriteHelper
{
    public static string WriteNumberWithSuffix(int? number, string suffix)
    {
        return number is null ? "N/A" : $"{number} {suffix}";
    }
}
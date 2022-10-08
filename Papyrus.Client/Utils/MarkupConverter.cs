using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Papyrus.Client.Utils;

public static class MarkupConverter
{
    public static MarkupString AsMarkup(this LocalizedString value) => new(value);
    public static MarkupString AsMarkup(this string value) => new(value);
}

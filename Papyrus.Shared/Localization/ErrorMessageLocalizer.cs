using KarcagS.Shared.Helpers;
using Microsoft.Extensions.Localization;

namespace Papyrus.Shared.Localization;

public class ErrorMessageLocalizer
{
    private static ErrorMessageLocalizer? Instance;
    private IStringLocalizer Localizer { get; set; } = default!;

    private ErrorMessageLocalizer()
    {
    }

    public static ErrorMessageLocalizer GetInstance()
    {
        Instance ??= new ErrorMessageLocalizer();

        return Instance;
    }

    public void AddLocalizer(IStringLocalizer localizer) => Localizer = localizer;

    public bool IsRegistered() => ObjectHelper.IsNotNull(Localizer);

    public string GetValue(string key, params string[] args) => Localizer[key, args];

    public const string RequiredKey = "Required";
    public const string EmailAddressKey = "EmailAddress";
    public const string MaxLengthKey = "MaxLength";
    public const string MinLengthKey = "MinLength";
    public const string ContainsAlphaKey = "ContainsAlpha";
    public const string ContainsCapitalAlphaKey = "ContainsCapitalAlpha";
    public const string ContainsNumericKey = "ContainsNumeric";
    public const string ContainsSpecialKey = "ContainsSpecial";
}

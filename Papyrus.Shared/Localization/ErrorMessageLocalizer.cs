using KarcagS.Shared.Helpers;
using Microsoft.Extensions.Localization;
using Papyrus.Shared.Attributes;

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
}

using KarcagS.Shared.Helpers;
using Papyrus.Shared.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Papyrus.Shared.Attributes;

public class LocalizedContainsAlphaAttribute : ValidationAttribute
{
    public override string FormatErrorMessage(string name)
    {
        var localizer = ErrorMessageLocalizer.GetInstance();

        if (localizer.IsRegistered())
        {
            return ErrorMessageLocalizer.GetInstance().GetValue(ErrorMessageLocalizer.ContainsAlphaKey);
        }

        return base.FormatErrorMessage(name);
    }

    public override bool IsValid(object? value)
    {
        if (ObjectHelper.IsNotNull(value) && value is string v)
        {
            Match m = Regex.Match(v, @"^(?=.*[a-z]).+$", RegexOptions.None);

            return m.Success;
        }

        return false;
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using KarcagS.Shared.Helpers;
using Papyrus.Shared.Localization;

namespace Papyrus.Shared.Attributes;

public class LocalizedContainsNumericAttribute : ValidationAttribute
{
    public override string FormatErrorMessage(string name)
    {
        var localizer = ErrorMessageLocalizer.GetInstance();

        if (localizer.IsRegistered())
        {
            return ErrorMessageLocalizer.GetInstance().GetValue(ErrorMessageLocalizer.ContainsNumericKey);
        }

        return base.FormatErrorMessage(name);
    }

    public override bool IsValid(object? value)
    {
        if (ObjectHelper.IsNotNull(value) && value is string v)
        {
            Match m = Regex.Match(v, @"^(?=.*\d).+$", RegexOptions.IgnoreCase);

            return m.Success;
        }

        return false;
    }
}

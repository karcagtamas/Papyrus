using Papyrus.Shared.Localization;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Attributes;

public class LocalizedRequiredAttribute : RequiredAttribute
{
    public override string FormatErrorMessage(string name)
    {
        var localizer = ErrorMessageLocalizer.GetInstance();

        if (localizer.IsRegistered())
        {
            return ErrorMessageLocalizer.GetInstance().GetValue(ErrorMessageLocalizer.RequiredKey);
        }

        return base.FormatErrorMessage(name);
    }
}

using Papyrus.Shared.Localization;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Attributes;

public class LocalizedMinLengthAttribute : MinLengthAttribute
{
    public LocalizedMinLengthAttribute(int length) : base(length)
    {

    }

    public override string FormatErrorMessage(string name)
    {
        var localizer = ErrorMessageLocalizer.GetInstance();

        if (localizer.IsRegistered())
        {
            return ErrorMessageLocalizer.GetInstance().GetValue(ErrorMessageLocalizer.MinLengthKey, Length.ToString());
        }

        return base.FormatErrorMessage(name);
    }
}

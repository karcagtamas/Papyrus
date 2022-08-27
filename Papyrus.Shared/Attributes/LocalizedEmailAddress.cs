using KarcagS.Shared.Attributes;
using Papyrus.Shared.Localization;

namespace Papyrus.Shared.Attributes;

public class LocalizedEmailAddress : NullableEmailAddressAttribute
{
    public override string FormatErrorMessage(string name)
    {
        var localizer = ErrorMessageLocalizer.GetInstance();

        if (localizer.IsRegistered())
        {
            return ErrorMessageLocalizer.GetInstance().GetValue(ErrorMessageLocalizer.EmailAddressKey);
        }

        return base.FormatErrorMessage(name);
    }
}

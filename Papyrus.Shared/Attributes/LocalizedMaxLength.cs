using Papyrus.Shared.Localization;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Attributes;

public class LocalizedMaxLengthAttribute : MaxLengthAttribute
{
    public LocalizedMaxLengthAttribute() : base()
    {

    }

    public LocalizedMaxLengthAttribute(int length) : base(length)
    {

    }

    public override string FormatErrorMessage(string name)
    {
        var localizer = ErrorMessageLocalizer.GetInstance();

        if (localizer.IsRegistered())
        {
            return ErrorMessageLocalizer.GetInstance().GetValue(ErrorMessageLocalizer.MaxLengthKey, Length.ToString());
        }

        return base.FormatErrorMessage(name);
    }
}

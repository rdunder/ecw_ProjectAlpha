using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Extensions;

public class RequiredGuidAttribute : ValidationAttribute
{
    public RequiredGuidAttribute()
    {
        
    }

    public override bool IsValid(object? value)
    {
        if (value is Guid guid)
        {
            return guid != Guid.Empty;
        }
        return false;
    }
}

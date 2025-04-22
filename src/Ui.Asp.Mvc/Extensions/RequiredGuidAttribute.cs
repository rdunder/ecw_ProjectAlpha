using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Extensions;

public class RequiredGuidAttribute : ValidationAttribute
{

    /// <summary>
    /// Used when a required field is Guid, because Guid can not be null!
    /// </summary>
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

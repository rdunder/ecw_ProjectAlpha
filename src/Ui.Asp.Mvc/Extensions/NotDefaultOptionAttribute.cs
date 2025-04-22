using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Extensions;

public class NotDefaultOptionAttribute : ValidationAttribute
{
    //  In a discussion with Claude Ai i came to the conclusion that this is a good idea
    //  when validating a select element mwith a disabled default option
    private readonly string _defaultValue;

    /// <summary>
    /// Used when default option on select input element is considered "not selected"
    /// </summary>
    /// <param name="defaultValue"></param>
    public NotDefaultOptionAttribute(string defaultValue = "")
    {
        _defaultValue = defaultValue;
        ErrorMessage = "";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue && stringValue == _defaultValue)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Extensions;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxFileSize">Maximum file size in MB</param>
    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize * 1024 * 1024;
    }

    public override bool IsValid(object? value)
    {
        if (value == null) { return true; }

        if (value is IFormFile file)
        {
            return file.Length <= _maxFileSize;
        }
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return base.FormatErrorMessage(name);
    }
}

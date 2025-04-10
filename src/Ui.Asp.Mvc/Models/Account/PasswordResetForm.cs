using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Models.Account;

public class PasswordResetForm
{
    [Display(Name = "New Password")]
    [Required(ErrorMessage = "You must enter a Password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password Requirements: \nMinimum 8 characters. \nAt least one uppercase letter. \nAt least one digit. \nAt least one special character")]
    public string Password { get; set; } = null!;


    [Display(Name = "Confirm new Password")]
    [Required(ErrorMessage = "You must confirm your Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Your Password do not match!")]
    public string ConfirmPassword { get; set; } = null!;

    public string Code { get; set; } = null!;
    public Guid Id { get; set; }
}

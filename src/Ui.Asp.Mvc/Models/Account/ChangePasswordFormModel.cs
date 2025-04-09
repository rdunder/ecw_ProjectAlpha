using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Models.Account;

public class ChangePasswordFormModel
{
    public Guid Id { get; set; }

    [Display(Name = "Old Password")]
    [Required(ErrorMessage = "You must enter a Password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password Requirements: \nMinimum 8 characters. \nAt least one uppercase letter. \nAt least one digit. \nAt least one special character")]
    public string OldPassword { get; set; } = null!;

    [Display(Name = "New Password")]
    [Required(ErrorMessage = "You must enter a Password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password Requirements: \nMinimum 8 characters. \nAt least one uppercase letter. \nAt least one digit. \nAt least one special character")]
    public string NewPassword { get; set; } = null!;


    [Display(Name = "Confirm New Password")]
    [Required(ErrorMessage = "You must confirm your Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Your Password do not match!")]
    public string ConfirmNewPassword { get; set; } = null!;
}

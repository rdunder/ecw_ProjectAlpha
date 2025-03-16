
using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;

public class UserDto
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "You must enter your First Name")]
    public string FirstName { get; set; } = null!;


    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "You must enter your Last Name")]
    public string LastName { get; set; } = null!;


    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "You must enter a valid Email Address")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email need to be formatted as <name@domain.com>")]
    public string Email { get; set; } = null!;


    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^(?:\+46\s?|0)\d(?:[\s]?\d){8,11}$", ErrorMessage = "Phonenumber needs to be formatted as:\n+46 701231234 OR\n0701231234")]
    public string? PhoneNumber { get; set; }


    [Display(Name = "Password")]
    [Required(ErrorMessage = "You must enter a Password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password Requirements: \nMinimum 8 characters. \nAt least one uppercase letter. \nAt least one digit. \nAt least one special character")]
    public string Password { get; set; } = null!;


    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "You must confirm your Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Your Password do not match!")]
    public string ConfirmPassword { get; set; } = null!;


    public bool AcceptTerms { get; set; }


    public string? Avatar { get; set; }

    public string? RoleName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public int? PostalCode { get; set; }

    public DateOnly? BirthDate { get; set; }
}

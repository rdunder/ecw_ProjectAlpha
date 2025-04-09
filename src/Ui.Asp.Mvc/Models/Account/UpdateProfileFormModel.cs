using Service.Dtos;
using System.ComponentModel.DataAnnotations;
using Ui.Asp.Mvc.Extensions;

namespace Ui.Asp.Mvc.Models.Account;

public class UpdateProfileFormModel
{
    public Guid Id { get; set; }

    [Display(Name = "Project Avatar", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
    [MaxFileSize(5, ErrorMessage = "Max filesize is 5 MB")]
    public IFormFile? File { get; set; }
    public string? Avatar { get; set; }



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

    public DateOnly? BirthDate { get; set; }

    [Display(Name = "Street Address", Prompt = "Enter Street Address")]
    [MinLength(4, ErrorMessage = "Address must be at least 4 characters")]
    public string? Address { get; set; }

    [Display(Name = "Postal Code", Prompt = "123")]
    [DataType(DataType.PostalCode)]
    [Range(1, 999999, ErrorMessage = "Postal code cannot be 0")]
    public int? PostalCode { get; set; }

    [Display(Name = "City", Prompt = "Enter City")]
    [MinLength(2, ErrorMessage = "City must be at least 2 characters")]
    public string? City { get; set; }

    public static implicit operator UserDto(UpdateProfileFormModel form) =>
        form is null
        ? null!
        : new UserDto
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            PhoneNumber = form.PhoneNumber,
            BirthDate = form.BirthDate,
            Avatar = form.Avatar,

            Address = (form.Address != null || form.PostalCode >= 10000 || form.City != null)
                ? new()
                {
                    Address = form.Address,
                    PostalCode = form.PostalCode,
                    City = form.City
                }
                : null
        };
}

using Service.Dtos;
using System.ComponentModel.DataAnnotations;
using Ui.Asp.Mvc.Extensions;

namespace Ui.Asp.Mvc.Models;

public class MemberFormViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Project Avatar", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
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



    [Display(Name = "Job Title")]
    [Required(ErrorMessage = "You must select a Role")]
    [NotDefaultOption(ErrorMessage = "You need to make a selection")]
    public string RoleName { get; set; } = null!;


    [Display(Name = "Street Address", Prompt = "Enter Street Address")]
    public string? Address { get; set; }

    [Display(Name = "Postal Code", Prompt = "123")]
    [DataType(DataType.PostalCode)]
    public int PostalCode { get; set; }

    [Display(Name = "City", Prompt = "Enter City")]
    public string? City { get; set; }




    public static implicit operator UserDto(MemberFormViewModel viewModel) =>
        viewModel is null
        ? null!
        : new UserDto
        {
            Id = viewModel.Id,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Email = viewModel.Email,
            PhoneNumber = viewModel.PhoneNumber,
            RoleName = viewModel.RoleName,

            BirthDate = viewModel.BirthDate,
            Avatar = viewModel.Avatar,
        };
}

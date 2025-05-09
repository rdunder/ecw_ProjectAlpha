﻿using Service.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Models.Auth;

public class RegisterViewModel
{
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


    [Range(typeof(bool), "true", "true", ErrorMessage = "You need to accept the terms anc conditions to use this site")]
    public bool AcceptTerms { get; set; }



    public static implicit operator UserDto(RegisterViewModel viewModel) =>
        viewModel is null
        ? null!
        : new UserDto
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Email = viewModel.Email,
            Password = viewModel.Password,

            Avatar = viewModel.Avatar,
        };
}

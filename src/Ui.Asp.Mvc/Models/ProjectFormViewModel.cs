using Service.Dtos;
using Service.Models;
using System.ComponentModel.DataAnnotations;
using Ui.Asp.Mvc.Extensions;

namespace Ui.Asp.Mvc.Models;

public class ProjectFormViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Project Name", Prompt = "Enter Project Name")]
    [Required(ErrorMessage = "You must enter a name")]
    public string Name { get; set; } = null!;

    [Display(Name = "Project Description", Prompt = "Describe the project")]
    [Required(ErrorMessage = "You must enter a description")]
    public string Description { get; set; } = null!;

    [Display(Name = "Starting Date")]
    [Required(ErrorMessage = "You must enter a starting date")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Display(Name = "Ending Date")]
    [Required(ErrorMessage = "You must enter a ending date")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Display(Name = "Budget", Prompt = "$ Enter Budget")]
    [Required(ErrorMessage = "You must enter a budget")]
    public decimal Budget { get; set; }


    [Display(Name = "Customer", Prompt = "Select a customer")]
    [Required(ErrorMessage = "You must select a customer")]
    [RequiredGuid(ErrorMessage = "You need to make a selection")]
    public Guid CustomerId { get; set; }


    public Guid StatusId { get; set; }


    [Display(Name = "Project Avatar", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
    public IFormFile? File { get; set; }
    public string? Avatar { get; set; }

    public List<UserModel>? Members { get; set; } = [];

    public List<Guid>? MemberIds { get; set; } = [];


    public static implicit operator ProjectDto(ProjectFormViewModel form) =>
        form is null
        ? null!
        : new ProjectDto
        {
            Id = form.Id,
            Name = form.Name,
            Description = form.Description,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            CustomerId = form.CustomerId,
            StatusId = form.StatusId,
            Budget = form.Budget,
            Avatar = form.Avatar,
            Users = form.Members,
            UsersIds = form.MemberIds,
        };
}



using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;

public class ProjectDto
{

    [Display(Name = "Project Name", Prompt = "Enter Project Name")]
    [Required(ErrorMessage = "You must enter a name")]
    public string Name { get; set; } = null!;

    [Display(Name = "Project Description", Prompt = "Describe the project")]
    [Required(ErrorMessage = "You must enter a description")]
    public string Description { get; set; } = null!;

    [Display(Name = "Starting Date", Prompt = "Enter Start Date")]
    [Required(ErrorMessage = "You must enter a starting date")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "Ending Date", Prompt = "Enter End Date")]
    [Required(ErrorMessage = "You must enter a ending date")]
    public DateOnly EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "$ Enter Budget")]
    [Required(ErrorMessage = "You must enter a budget")]
    public decimal Budget { get; set; }

    public string? Avatar { get; set; }

    public Guid StatusId { get; set; }
    public Guid CustomerId { get; set; }
}

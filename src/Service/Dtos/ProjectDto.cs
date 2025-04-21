using Microsoft.AspNetCore.Http;
using Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;
public class ProjectDto
{
    public Guid Id { get; set; }

    [Display(Name = "Project Name", Prompt = "Enter Project Name")]
    [Required(ErrorMessage = "You must enter a name")]
    public string Name { get; set; } = null!;

    [Display(Name = "Project Description", Prompt = "Describe the project")]
    [Required(ErrorMessage = "You must enter a description")]
    [RegularExpression(@"^(\s*\S+\s+){4,}\S+.*$", ErrorMessage = "Describe the project in at least 5  words")]
    public string Description { get; set; } = null!;

    [Display(Name = "Starting Date")]
    [Required(ErrorMessage = "You must enter a starting date")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Display(Name = "Ending Date")]
    [Required(ErrorMessage = "You must enter a ending date")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Display(Name = "Budget", Prompt = "$ Enter Budget")]
    [Required(ErrorMessage = "You must enter a budget")]
    public decimal Budget { get; set; }


    [Display(Name = "Customer", Prompt = "Select a customer")]
    [Required(ErrorMessage = "You must select a customer")]
    public Guid CustomerId { get; set; }


    [Display(Name = "Status", Prompt = "Select a Status")]
    [Required(ErrorMessage = "You must select a Status")]
    public Guid StatusId { get; set; }


    [Display(Name = "Project Avatar", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
    public IFormFile? File { get; set; }


    public string? Avatar { get; set; }

    public List<UserModel>? Users { get; set; } = [];
    public List<Guid>? UsersIds { get; set; } = [];
}

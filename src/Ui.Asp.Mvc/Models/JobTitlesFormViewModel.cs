using Service.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Models;

public class JobTitlesFormViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Job Title", Prompt = "Enter customer name")]
    [Required(ErrorMessage = "You must enter a name")]
    public string Title { get; set; } = null!;

    public static implicit operator JobTitleDto(JobTitlesFormViewModel form) =>
        new JobTitleDto
        {
            Id = form.Id,
            Title = form.Title,
        };
}

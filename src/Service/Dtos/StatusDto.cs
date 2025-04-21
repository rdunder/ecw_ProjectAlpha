using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;
public class StatusDto
{
    [Display(Name = "Status Name")]
    [Required(ErrorMessage = "You must enter a Name")]
    public string StatusName { get; set; } = null!;
}

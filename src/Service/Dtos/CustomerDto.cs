

using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;

public class CustomerDto
{
    [Display(Name = "Customer Name", Prompt = "Enter customer name")]
    [Required(ErrorMessage = "You must enter a name")]
    public string CustomerName { get; set; } = null!;

    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "You must enter a valid Email Address")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email need to be formatted as <name@domain.com>")]
    public string Email { get; set; } = null!;
}

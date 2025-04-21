using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;
public class UserAddressDto
{
    public Guid UserEntityId { get; set; }

    [Display(Name = "Street Address")]
    [Required(ErrorMessage = "You must enter a street address")]
    public string Address { get; set; } = null!;

    [Display(Name = "City")]
    [Required(ErrorMessage = "You must enter a city")]
    public string City { get; set; } = null!;

    [Display(Name = "Postal Code")]
    [Required(ErrorMessage = "You must enter a postal code")]
    public int? PostalCode { get; set; }
}

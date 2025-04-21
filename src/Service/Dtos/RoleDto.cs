using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;
public class RoleDto
{
    [Display(Name = "Role Name", Prompt = "Enter Role Name")]
    [Required(ErrorMessage = "You must enter a name")]
    public string RoleName { get; set; } = null!;
}

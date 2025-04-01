
using System.ComponentModel.DataAnnotations;

namespace Service.Dtos;

public class UserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    public string? UserName { get; set; }

    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = null!;


    public string? Avatar { get; set; }

    public string? RoleName { get; set; }

    

    public DateOnly? BirthDate { get; set; }

}



namespace Service.Models;

public class User
{
    public Guid Id { get; set; }   
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Avatar { get; set; }
    public DateOnly? BirthDate { get; set; }


    public string? RoleName { get; set; }
}

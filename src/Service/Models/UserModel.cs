

using Data.Entities;
using Service.Dtos;

namespace Service.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Avatar { get; set; }
    public DateOnly? BirthDate { get; set; }

    public UserAddressModel? Address { get; set; }
    public string? RoleName { get; set; }   
}

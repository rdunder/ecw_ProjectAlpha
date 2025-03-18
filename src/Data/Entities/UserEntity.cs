using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserEntity : IdentityUser<int>
{
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [MaxLength(20)]
    public override string? PhoneNumber { get; set; }

    public string? Avatar { get; set; }   

    public DateOnly? BirthDate { get; set; }


    [NotMapped]
    public string? RoleName { get; set; }

    public RoleEntity? Role { get; set; }
    public UserAddressEntity? Address { get; set; }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserEntity : IdentityUser<Guid>
{
    [MaxLength(50)]
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    [MaxLength(20)]
    [ProtectedPersonalData]
    public override string? PhoneNumber { get; set; }

    [PersonalData]
    public string? Avatar { get; set; }

    [ProtectedPersonalData]
    public DateOnly? BirthDate { get; set; }


    public Guid JobTitleId { get; set; }
    public JobTitleEntity? JobTitle { get; set; }


    public ICollection<ProjectEntity> Projects { get; set; } = [];

    public UserAddressEntity? Address { get; set; }


    [NotMapped]
    public string? RoleName { get; set; }

    [NotMapped]
    public string? Title {  get; set; }
}

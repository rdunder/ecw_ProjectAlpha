using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class UserAddressEntity
{
    [Key]
    public Guid UserEntityId { get; set; }

    [MaxLength(50)]
    [ProtectedPersonalData]
    public string? Address { get; set; }

    [MaxLength(50)]
    [PersonalData]
    public string? City { get; set; }

    [MaxLength(50)]
    [PersonalData]
    public int? PostalCode { get; set; }



    public UserEntity? User { get; set; }
}
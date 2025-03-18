using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class UserAddressEntity
{
    [Key]
    public int UserEntityId { get; set; }

    [MaxLength(50)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? City { get; set; }

    [MaxLength(50)]
    public int? PostalCode { get; set; }

    public UserEntity? User { get; set; }
}
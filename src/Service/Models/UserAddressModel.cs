using Data.Entities;

namespace Service.Models;
public class UserAddressModel
{
    public Guid UserEntityId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public int? PostalCode { get; set; }

    public UserEntity? User { get; set; }
}

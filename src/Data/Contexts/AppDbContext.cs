

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<UserEntity, RoleEntity, int> (options)
{
    public DbSet<UserAddressEntity> UserAddresses { get; set; }
}

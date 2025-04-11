

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<UserEntity, RoleEntity, Guid> (options)
{
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<StatusEntity> Statuses { get; set; }
    public DbSet<UserAddressEntity> UserAddresses { get; set; }
    public DbSet<JobTitleEntity> JobTitles { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

       builder.Entity<UserEntity>()
            .HasOne(user => user.Address)
            .WithOne(address => address.User)
            .HasForeignKey<UserAddressEntity>(a => a.UserEntityId);
    }
}

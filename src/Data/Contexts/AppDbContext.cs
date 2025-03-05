

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

class AppDbContext(DbContextOptions options) : IdentityDbContext<UserEntity, RoleEntity, int> (options)
{
}

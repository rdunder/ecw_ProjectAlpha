using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data.Contexts;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
        .AddUserSecrets<AppDbContextFactory>()
        .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalDb"));

        return new AppDbContext(optionsBuilder.Options);
    }
}

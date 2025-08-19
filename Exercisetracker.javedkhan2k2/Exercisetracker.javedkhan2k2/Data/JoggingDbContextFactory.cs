using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Exercisetacker.Data;

public class JoggingDbContextFactory : IDesignTimeDbContextFactory<JoggingDbContext>
{
    public JoggingDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<JoggingDbContextFactory>()
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<JoggingDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);

        return new JoggingDbContext(optionsBuilder.Options);
    }
}
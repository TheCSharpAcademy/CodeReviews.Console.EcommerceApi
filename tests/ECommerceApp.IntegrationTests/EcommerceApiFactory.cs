using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ECommerceApp.IntegrationTests;

public sealed class EcommerceApiFactory : WebApplicationFactory<ECommerceApp.RyanW84.Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            // Keep tests isolated and fast: swap DB for an in-memory SQLite connection
            // and remove the migration/seeding hosted service.
            _connection ??= new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var hostedServiceDescriptors = services
                .Where(d =>
                    d.ServiceType == typeof(Microsoft.Extensions.Hosting.IHostedService)
                    && d.ImplementationType == typeof(DatabaseInitializerHostedService)
                )
                .ToList();

            foreach (var descriptor in hostedServiceDescriptors)
                services.Remove(descriptor);

            services.RemoveAll<DbContextOptions<ECommerceDbContext>>();

            // Use DbContext (not pooling) for tests to avoid shared state issues
            services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseSqlite(_connection);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection?.Dispose();
            _connection = null;
        }
    }
}

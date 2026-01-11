using ECommerceApp.RyanW84.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Hosting;

/// <summary>
/// Applies migrations at startup. In Development, optionally resets and seeds the database.
/// Keeps Program.cs lean and satisfies SRP.
/// </summary>
public class DatabaseInitializerHostedService(IServiceProvider services,
    IWebHostEnvironment env,
    ILogger<DatabaseInitializerHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        try
        {
            // For development, use a reset flag instead of always deleting the database
            // This avoids the expensive EnsureDeletedAsync call on every startup
            // Only reset if the environment variable is explicitly set
            if (env.IsDevelopment() && IsResetRequested())
            {
                logger.LogInformation("Development mode: resetting database as requested...");
                await db.Database.EnsureDeletedAsync(cancellationToken);
            }

            await db.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Database migrations applied.");

            if (env.IsDevelopment() && IsResetRequested())
            {
                db.SeedData();
                await db.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Database seeded with initial data.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// Checks if database reset is requested via environment variable.
    /// Set ECOMMERCE_RESET_DB=true to reset the database on startup.
    /// </summary>
    private static bool IsResetRequested()
    {
        var resetEnv = Environment.GetEnvironmentVariable("ECOMMERCE_RESET_DB");
        return resetEnv?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}


using ExerciseTrackerAPI.Services;
using FluentMigrator.Runner;

namespace ExerciseTrackerAPI.Data;

public static class ApplicationDbInitializer
{
    public static void Initialize(WebApplication app)
    {
        DatabaseService.Initialize();

        using var scope = app.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}

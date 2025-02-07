using ExerciseTrackerAPI.Repositories.ExerciseRepositories;
using ExerciseTrackerAPI.Repositories.ExerciseTypeRepositories;

namespace ExerciseTrackerAPI.Services;

public static class RepositoryService
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration["Repositories:ExerciseType"] switch
        {
            "AdoNet" => services.AddScoped<IExerciseTypeRepository, AdoNetExerciseTypeRepository>(),
            "Dapper" => services.AddScoped<IExerciseTypeRepository, DapperExerciseTypeRepository>(),
            _ => services.AddScoped<IExerciseTypeRepository, EfCoreExerciseTypeRepository>()
        };

        _ = configuration["Repositories:Exercise"] switch
        {
            "AdoNet" => services.AddScoped<IExerciseRepository, AdoNetExerciseRepository>(),
            "Dapper" => services.AddScoped<IExerciseRepository, DapperExerciseRepository>(),
            _ => services.AddScoped<IExerciseRepository, EfCoreExerciseRepository>()
        };

        return services;
    }
}

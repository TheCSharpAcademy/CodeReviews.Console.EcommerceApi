using Microsoft.Extensions.DependencyInjection;
using ExerciseTracker.kwm0304.Services;
using ExerciseTracker.kwm0304.Repositories;
using ExerciseTracker.kwm0304.Controllers;
using ExerciseTracker.kwm0304.Data;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
namespace ExerciseTracker.kwm0304;

public class Program
{
    static async Task Main()
    {
        DotEnv.Load();
        var services = new ServiceCollection();
        ConfigureServices(services);
        var provider = services.BuildServiceProvider();
        var session = provider.GetRequiredService<AppSession>();
        await session.OnStart();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        services.AddDbContext<ExerciseDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddTransient<AppSession>();
        services.AddTransient<ExerciseRepository>();
        services.AddTransient<ExerciseService>();
        services.AddTransient<ExerciseController>();
    }
}
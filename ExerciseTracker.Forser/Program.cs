using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExerciseTracker.Forser;

internal static class Program
{
    private static readonly IConfiguration configuration;
    static Program()
    {
        configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
    static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();

        IServiceProvider exerciseServiceProvider = host.Services;
        IExerciseController? exerciseController = exerciseServiceProvider.GetService<IExerciseController>();
        exerciseController!.Run();
    }
    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => {
            services.AddDbContext<ExerciseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ExerciseConnection"));
            })
            .AddScoped<IExerciseReposoitory, ExerciseRepository>()
            .AddScoped<IExerciseService, ExerciseService>()
            .AddScoped<IExerciseController, ExerciseController>()
            .AddScoped<IUserInterface, UserInterface>();
        });
    }
}
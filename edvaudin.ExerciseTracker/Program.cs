using edvaudin.ExerciseTracker.Context;
using edvaudin.ExerciseTracker.Controllers;
using edvaudin.ExerciseTracker.Input;
using edvaudin.ExerciseTracker.Repositories;
using edvaudin.ExerciseTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace edvaudin.ExerciseTracker;

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

        var serviceProvider = host.Services;
        var exerciseController = serviceProvider.GetService<IExerciseController>();
        exerciseController!.Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
        {
            services
            .AddDbContext<ExerciseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            })
            .AddScoped<IExerciseController, ExerciseController>()
            .AddScoped<IExerciseService, ExerciseService>()
            .AddScoped<IExerciseRepository, ExerciseRepository>()
            .AddScoped<IUserInput, UserInput>();
        });
    }
}
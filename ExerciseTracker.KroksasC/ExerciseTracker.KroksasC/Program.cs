using ExerciseTracker.KroksasC.Controllers;
using ExerciseTracker.KroksasC.Models;
using ExerciseTracker.KroksasC.Repositaries;
using ExerciseTracker.KroksasC.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExerciseTracker.KroksasC;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
        .AddDbContext<ExerciseTrackerDbContext>()
        .AddScoped(typeof(IRepository<>), typeof(Repository<>))
        .AddScoped<IUnitOfWork, UnitOfWork>()
        .AddScoped<ExerciseService>()
        .AddScoped<ExerciseController>()
        .BuildServiceProvider();

        ExerciseController menu = new ExerciseController(serviceProvider.GetRequiredService<ExerciseService>());

        await menu.Run();
    }
}


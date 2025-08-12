using ExerciseTracker.Data;
using ExerciseTracker.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExerciseTracker;

internal class Program
{
    static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddDbContext<ExerciseContext>();
        builder.Services.AddScoped<UserInput>();
        builder.Services.AddScoped<ExerciseController>();
        builder.Services.AddScoped<Menu>();
        builder.Services.AddScoped<IExerciseService, ExerciseService>();
        builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();

        using IHost app = builder.Build();

        var dbContext = app.Services.GetRequiredService<ExerciseContext>();

        // dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var _menu = app.Services.GetRequiredService<Menu>();

        _menu.MainMenu();
    }
}

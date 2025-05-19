using ExerciseTracker.KamilKolanowski.Controllers;
using ExerciseTracker.KamilKolanowski.Models.Data;
using ExerciseTracker.KamilKolanowski.Repositories;
using ExerciseTracker.KamilKolanowski.Services;
using ExerciseTracker.KamilKolanowski.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExerciseTracker.KamilKolanowski;

class Program
{
    static void Main()
    {
        var builder = Host.CreateApplicationBuilder();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ExerciseTrackerDbContext>(options =>
            options.UseSqlServer(connectionString)
        );
        builder.Services.AddTransient<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddTransient<ExerciseService>();
        builder.Services.AddTransient<ExerciseController>();
        builder.Services.AddTransient<MainInterface>();
        builder.Services.AddTransient<UserInputService>();

        builder.Logging.ClearProviders(); 
        var app = builder.Build();
        var scope = app.Services.CreateScope();
        
        var main = scope.ServiceProvider.GetRequiredService<MainInterface>();
        main.Start();

    }
}

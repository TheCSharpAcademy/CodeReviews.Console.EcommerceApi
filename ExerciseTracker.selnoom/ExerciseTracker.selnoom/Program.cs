using ExerciseTracker.Data;
using ExerciseTracker.selnoom.Controllers;
using ExerciseTracker.selnoom.Data;
using ExerciseTracker.selnoom.Menu;
using ExerciseTracker.selnoom.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Warning);
            })
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ExerciseDbContext>(options =>
                    options.UseSqlite(connectionString));
                services.AddTransient<WeightExerciseRepository>();
                services.AddTransient<WeightExerciseService>();
                services.AddTransient<WeightExerciseController>();
                services.AddTransient<Menu>();
            })
            .Build();

using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ExerciseDbContext>();
    context.Database.Migrate();

    var menu = scope.ServiceProvider.GetRequiredService<Menu>();
    await menu.ShowMenu();
}
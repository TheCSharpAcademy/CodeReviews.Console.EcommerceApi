using ExerciceTracker.Controllers;
using ExerciceTracker.Data.Models;
using ExerciceTracker.Data.Repositories;
using ExerciceTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<ExerciseDbContext>(options =>
            options.UseSqlServer("Server=DESKTOP-ETA4JL7;Database=Exercices;Trusted_Connection=True;Integrated Security=True;Encrypt=False;"));

        services.AddTransient<IExerciseRepository, ExerciseRepository>();
        services.AddTransient<ExerciseService>();
        services.AddTransient<ExerciseController>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ExerciseDbContext>();
    var exerciseController = services.GetRequiredService<ExerciseController>();

    exerciseController.MainMenu();
}

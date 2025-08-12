using Microsoft.Extensions.Hosting;
using Kmakai.ExerciseTracker.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Kmakai.ExerciseTracker.Controllers;
using Kmakai.ExerciseTracker.Repositories;
using Kmakai.ExerciseTracker.Services;
using Microsoft.Extensions.Logging;
using Kmakai.ExerciseTracker;

var connectionString = "Server=.;Database=ExerciseTracker;TrustServerCertificate=true;Trusted_Connection=True";
IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((services) =>
    {
        services.AddDbContext<ExerciseContext>(options => options.UseSqlServer(connectionString));
        services.AddSingleton<IExerciseRepository, ExerciseRepository>();
        services.AddSingleton<IExerciseService, ExerciseService>();
        services.AddSingleton<IExerciseController, ExerciseController>();
        services.AddSingleton<Tracker>();
    }).ConfigureLogging(logging =>
    {
        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    }).UseConsoleLifetime()
    .Build();

var services = host.Services;

var tracker = services.GetRequiredService<Tracker>();
tracker.Run();
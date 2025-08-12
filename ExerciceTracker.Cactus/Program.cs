using ExerciseTracker.Cactus.Controller;
using ExerciseTracker.Cactus.Data;
using ExerciseTracker.Cactus.Data.Interfaces;
using ExerciseTracker.Cactus.Data.Repositories;
using ExerciseTracker.Cactus.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = new HostBuilder()
        .ConfigureServices((hostContext, services) =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("App.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddDbContext<ExerciseDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LocalDbConnection")));

            services.AddTransient<IExerciseRepository, ExerciseRepository>();
            services.AddTransient<ExerciseService>();
            services.AddTransient<ExerciseController>();
        })
        .Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var controller = services.GetRequiredService<ExerciseController>();
            await controller.MainMenu();
        }
    }
}

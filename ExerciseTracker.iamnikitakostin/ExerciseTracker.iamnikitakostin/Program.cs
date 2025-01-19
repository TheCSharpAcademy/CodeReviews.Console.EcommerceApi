using ExerciseTracker.iamnikitakostin;
using ExerciseTracker.iamnikitakostin.Controllers;
using ExerciseTracker.iamnikitakostin.Data;
using ExerciseTracker.iamnikitakostin.Repositories;
using ExerciseTracker.iamnikitakostin.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program : ConsoleHelper
{
    static void Main()
    {
        try
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string? connectionString = config.GetConnectionString("DefaultConnection");
            if (connectionString == null)
            {
                ErrorMessage("Dear user, please ensure that you have your api connection string set up in the appsettings.json.");
                return;
            }

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(config);
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<ExerciseService>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<ExerciseController>();

            var serviceProvider = services.BuildServiceProvider();
            var userInterface = serviceProvider.GetRequiredService<ExerciseController>();
            userInterface.MainMenu();
        }
        catch (Exception ex)
        {
            ErrorMessage(ex.Message);
        }
    }
}
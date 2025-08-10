using ExerciseTracker.Brozda.Data;
using ExerciseTracker.Brozda.Repositories;
using ExerciseTracker.Brozda.Services;
using ExerciseTracker.Brozda.Services.Interfaces;
using ExerciseTracker.Brozda.UserInteraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExerciseTracker.Brozda
{
    internal class Program
    {
        private static async Task Main()
        {
            var services = new ServiceCollection();

            SetServices(services);

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<ExerciseController>();
            await app.Run();
        }

        public static void SetServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection")!;


            services.AddSingleton<IUserInputOutput>(new UserInputOutput());


            services.AddDbContext<ExerciseTrackerContext>();

            services.AddScoped<WeightExerciseRepository>();
            services.AddScoped<CardioExerciseRepository>(sp => new CardioExerciseRepository(connectionString));

            services.AddScoped<IWeightExerciseService>(sp => new ExerciseService(
                sp.GetRequiredService<WeightExerciseRepository>()));

            services.AddScoped<ICardioExerciseService>(sp => new ExerciseService(
                sp.GetRequiredService<CardioExerciseRepository>()));

            services.AddScoped<ExerciseController>();
        }
    }
}
using ExerciseTracker.Brozda.Data;
using ExerciseTracker.Brozda.Repositories;
using ExerciseTracker.Brozda.Repositories.Interfaces;
using ExerciseTracker.Brozda.Services;
using ExerciseTracker.Brozda.Services.Interfaces;
using ExerciseTracker.Brozda.UserInteraction;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Dapper;

namespace ExerciseTracker.Brozda
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            
            SetServices(services);

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<ExerciseController>();
            await app.Run();

        }
        public static void SetServices(IServiceCollection services) 
        {
            services.AddSingleton<IUserInputOutput>(new UserInputOutput());

            services.AddDbContext<ExerciseTrackerContext>();

            services.AddScoped<WeightExerciseRepository>();
            services.AddScoped<CardioExerciseRepository>();
            
            services.AddScoped<IWeightExerciseService>(sp => new ExerciseService(
                sp.GetRequiredService<WeightExerciseRepository>()));

            services.AddScoped<ICardioExerciseService>(sp => new ExerciseService(
                sp.GetRequiredService<CardioExerciseRepository>()));

            services.AddScoped<ExerciseController>();

        }
    }
}

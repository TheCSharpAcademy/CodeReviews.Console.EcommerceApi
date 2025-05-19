using ExerciseTracker.Brozda.Data;
using ExerciseTracker.Brozda.Models;
using ExerciseTracker.Brozda.Repositories;
using ExerciseTracker.Brozda.Repositories.Interfaces;
using ExerciseTracker.Brozda.Services;
using ExerciseTracker.Brozda.Services.Interfaces;
using ExerciseTracker.Brozda.UserInteraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using System.Resources;
using System.Text.Json;

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

            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<ExerciseController>();

        }
    }
}

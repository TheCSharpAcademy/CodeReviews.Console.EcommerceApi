using ExerciseTracker.KamilKolanowski.Controllers;
using ExerciseTracker.KamilKolanowski.Interfaces;
using ExerciseTracker.KamilKolanowski.Models;
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
        var modelBuilder = new ModelBuilder();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ExerciseTrackerDbContext>(options =>
            options.UseSqlServer(connectionString)
        );
        
        // builder.Services.AddTransient<IExerciseRepository, ExerciseRepository>(); // EF Core implementation;
        // builder.Services.AddTransient<IDapperExerciseRepository, DapperExerciseRepository>(); // Dapper implementation;
        builder.Services.AddTransient<ExerciseRepository>();         // EF Core
        builder.Services.AddTransient<DapperExerciseRepository>();   // Dapper
        builder.Services.AddTransient<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddTransient<IExerciseRepositoryFactory, ExerciseRepositoryFactory>();
        
        builder.Services.AddTransient<ExerciseService>();
        builder.Services.AddTransient<ExerciseController>();
        builder.Services.AddTransient<MainInterface>();
        builder.Services.AddTransient<UserInputService>();

        modelBuilder.Entity<Exercise>().Property(e => e.Comment).HasMaxLength(200);
        modelBuilder.Entity<Exercise>().Property(e => e.Name).HasMaxLength(100);
        modelBuilder.Entity<Exercise>().ToTable("ExerciseTracker", schema: "TCSA");


        builder.Logging.ClearProviders();
        var app = builder.Build();
        var scope = app.Services.CreateScope();

        var main = scope.ServiceProvider.GetRequiredService<MainInterface>();
        main.Start();
    }
}

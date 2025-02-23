using Data;
using Data.Repositories;
using ExerciseTracker.Controllers;
using ExerciseTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExerciseTracker;

public class Startup {
    public static IHost CreateHost(string[] args) {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
        
        builder.Services.AddDbContext<ExerciseEFDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddTransient<IExerciseRepository, ExerciseEFRepository>();
        builder.Services.AddTransient<App>(); 
        builder.Services.AddTransient<MainMenuController>();
        builder.Services.AddTransient<ExerciseService>();

        IHost host = builder.Build();
        return host;
    }
}
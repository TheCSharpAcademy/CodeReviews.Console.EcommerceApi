using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Services;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace ExerciseTrackerAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        ApplicationDbConnection.Initialize(builder);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ApplicationDbConnection.ConnectionString));

        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer2016()
                .WithGlobalConnectionString(ApplicationDbConnection.ConnectionString)
                .ScanIn(currentAssembly).For.Migrations());

        builder.Services.AddRepositories(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        ApplicationDbInitializer.Initialize(app);

        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference();
            app.MapOpenApi();
        }

        app.MapControllers();

        app.Run();
    }
}

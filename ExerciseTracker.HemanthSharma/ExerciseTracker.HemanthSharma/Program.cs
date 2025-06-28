
using ExerciseTracker.Study;
using ExerciseTracker.Study.Models;
using ExerciseTracker.Study.Repositories;
using ExerciseTracker.Study.Services;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.HemanthSharma
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ContextClass>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddScoped<IRepository<Exercise>, RepositoryDapper>();
            // builder.Services.AddScoped<IRepository<ExerciseDto>, RepositoryClass<ExerciseDto>>();
            builder.Services.AddScoped<IRepository<ExerciseShift>, RepositoryClass<ExerciseShift>>();
            builder.Services.AddScoped<IService<Exercise>, ServiceClass<Exercise>>();
            //builder.Services.AddScoped<IService<ExerciseDto>, ServiceClass<ExerciseDto>>();
            builder.Services.AddScoped<IService<ExerciseShift>, ServiceClass<ExerciseShift>>();
            builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("ConnectionStrings"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

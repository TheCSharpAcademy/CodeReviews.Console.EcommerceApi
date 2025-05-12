using ExerciseTracker.Brozda.Data;
using ExerciseTracker.Brozda.Models;
using ExerciseTracker.Brozda.Repositories;
using ExerciseTracker.Brozda.Services;
using ExerciseTracker.Brozda.UserInteraction;
using Microsoft.EntityFrameworkCore;
using System.Resources;
using System.Text.Json;

namespace ExerciseTracker.Brozda
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserInput ui = new UserInput();
            ExcerciseTrackerContext _context = new ExcerciseTrackerContext();
            ExerciseRepository repo = new ExerciseRepository(_context);
            ExerciseService svc = new ExerciseService(repo);
            ExerciseController app = new ExerciseController(ui, svc);

            //app.Run();
            var projectRoot = Environment.GetEnvironmentVariable("PROJECT_ROOT");

            var path = Path.Combine(projectRoot, "Resources","SeedData.json");
    
            if (File.Exists(path))
            {
                var rawData = File.ReadAllText(path);
                var deserialized = JsonSerializer.Deserialize<SeedData>(rawData);
            }
        }
    }
}

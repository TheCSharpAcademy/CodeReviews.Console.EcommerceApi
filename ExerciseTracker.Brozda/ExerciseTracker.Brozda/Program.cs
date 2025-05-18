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
        static async Task Main(string[] args)
        {
            UserInputOutput ui = new UserInputOutput();
            ExerciseTrackerContext _context = new ExerciseTrackerContext();
            ExerciseRepository repo = new ExerciseRepository(_context);
            ExerciseService svc = new ExerciseService(repo);
            ExerciseController app = new ExerciseController(ui, svc);

            await app.Run();
            
        }
    }
}

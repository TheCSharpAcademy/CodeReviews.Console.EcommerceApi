using ExerciseTracker.Brozda.Data;
using ExerciseTracker.Brozda.Repositories;
using ExerciseTracker.Brozda.Services;
using ExerciseTracker.Brozda.UserInteraction;

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
            DateTime start = DateTime.Now;
            DateTime end = start.AddMinutes(30);

            long duration = (long)(end - start).TotalSeconds;
            Console.WriteLine($"It took: {TimeSpan.FromSeconds(duration)}");
        }
    }
}

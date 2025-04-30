using System.Threading.Tasks;

namespace ExerciseTracker.Call911plz;

class Program
{
    static async Task Main(string[] args)
    {
        ExerciseContext context = new();
        ExerciseRepository exerciseRepository = new(context);
    }
}

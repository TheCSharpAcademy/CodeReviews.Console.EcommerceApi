using System.Threading.Tasks;

namespace ExerciseTrackerUI;

class Program
{
  static async Task Main(string[] args)
  {
    var userInput = new UserInput();
    await userInput.Main();
  }
}
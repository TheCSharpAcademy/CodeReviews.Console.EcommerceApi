using ExerciceTracker.Services;

namespace ExerciceTracker.Controllers
{
    internal class ExerciseController
    {
        private readonly ExerciseService _exerciseService;

        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        internal void MainMenu()
        {
            bool isAppRunning = true;

            while (isAppRunning)
            {
                Console.WriteLine("\nChoose: \n");
                Console.WriteLine("1 to add an exercise\n");
                Console.WriteLine("2 to delete an exercise\n");
                Console.WriteLine("3 to see all exercises\n");
                Console.WriteLine("Q to quit\n");

                string userInput = Console.ReadLine();
                switch (userInput.ToLower())
                {
                    case "1":
                        _exerciseService.AddService();
                        break;
                    case "2":
                        _exerciseService.DeleteService();
                        break;
                    case "3":
                        _exerciseService.GetAllService();
                        break;
                    case "q":
                        isAppRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                } 
            }
        }
    }
}

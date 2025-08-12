using Spectre.Console;

namespace ExerciseTracker.JsPeanut
{
    class Program
    {
        public static bool exit;
        public static void Main(string[] args)
        {
            while(!exit)
            {
                MainMenu();
            }
        }

        public static void MainMenu()
        {
            ExerciseController exerciseController = new ExerciseController();
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("------------------- Exercise Tracker ------------------- \n\nWelcome! What do you want to do?")
                .AddChoices(
                    MenuOptions.InsertExerciseSession,
                    MenuOptions.DeleteExerciseSession,
                    MenuOptions.GetExerciseSession,
                    MenuOptions.GetAllExerciseSessions,
                    MenuOptions.UpdateExerciseSession,
                    MenuOptions.Quit));

            switch (option)
            {
                case MenuOptions.InsertExerciseSession:
                    exerciseController.InsertExercise();
                    break;
                case MenuOptions.DeleteExerciseSession:
                    exerciseController.Delete();
                    break;
                case MenuOptions.GetAllExerciseSessions:
                    exerciseController.ReadExercises();
                    Console.WriteLine("\nPress any key to go back to the main menu.");
                    Console.ReadKey();
                    MainMenu();
                    break;
                case MenuOptions.GetExerciseSession:
                    exerciseController.ReadExercise();
                    break;
                case MenuOptions.UpdateExerciseSession:
                    exerciseController.Update();
                    break;
                case MenuOptions.Quit:
                    Console.Clear();
                    Console.WriteLine("Goodbye!");
                    exit = true;
                    break;
            }
        }
    }
    enum MenuOptions
    {
        InsertExerciseSession,
        GetAllExerciseSessions,
        GetExerciseSession,
        DeleteExerciseSession,
        UpdateExerciseSession,
        Quit
    }
}
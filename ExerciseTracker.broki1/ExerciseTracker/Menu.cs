namespace ExerciseTracker;

internal class Menu
{
    private readonly UserInput _userInput;
    private readonly ExerciseController _controller;

    public Menu(UserInput userInput, ExerciseController controller)
    {
        _userInput = userInput;
        _controller = controller;
    }

    public void MainMenu()
    {
        var exitApplication = false;

        while (!exitApplication)
        {
            Console.Clear();
            var userInput = this._userInput.GetMainMenuInput();

            switch (userInput)
            {
                case '1':
                    try
                    {
                        this._controller.AddExerciseSession();
                        Console.WriteLine("Exercise added successfully. Press any key to continue.");
                        Console.ReadKey();
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                    {
                        Console.WriteLine("\nExercise end date/time cannot be before exercise start date/time.\nPress any key to continue.");
                        Console.ReadKey();
                    }
                    break;
                case '2':
                    this._controller.ViewAllExercises();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    break;
                case '3':
                    try
                    {
                        this._controller.UpdateExercise();
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                    {
                        Console.WriteLine("\nInvalid exercise date range.\nPress any key to continue.");
                        Console.ReadKey();
                    }
                    break;
                case '4':
                    this._controller.DeleteExercise();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    break;
                case '5':
                    Console.WriteLine("Goodbye!");
                    exitApplication = true;
                    break;
            }
        }
    }
}

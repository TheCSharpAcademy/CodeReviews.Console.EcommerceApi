using edvaudin.ExerciseTracker.Repositories;

namespace edvaudin.ExerciseTracker.Input;

internal class UserInput : IUserInput
{
    private readonly IExerciseRepository exerciseRepository;
    public UserInput(IExerciseRepository exerciseRepository)
    {
        this.exerciseRepository = exerciseRepository;
    }
    public DateTime GetStartTime()
    {
        string input = Console.ReadLine();

        while (!Validator.IsValidDateInput(input))
        {
            Console.WriteLine("\nInvalid date and time. Use the format: dd/MM/yyyy HH:mm:ss.");
            input = Console.ReadLine();
        }
        return Validator.ConvertToDate(input);
    }

    public DateTime GetEndTime(DateTime startTime)
    {
        string input = Console.ReadLine();

        while (!Validator.IsValidDateInput(input))
        {
            Console.WriteLine("\nInvalid date and time. Use the format: dd/MM/yyyy HH:mm:ss.");
            input = Console.ReadLine();
        }
        if (!Validator.IsDateAfterStartTime(input, startTime))
        {
            Console.WriteLine("\nYou cannot have finished coding before you started! Enter a different end time.");
            GetEndTime(startTime);
        }
        return Validator.ConvertToDate(input);
    }

    public string GetOption()
    {
        string input = Console.ReadLine();
        while (!Validator.IsValidOption(input))
        {
            Console.Write("\nThis is not a valid input. Please enter one of the above options: ");
            input = Console.ReadLine();
        }
        return input;
    }
    public int GetId()
    {
        bool validIdEntered = false;
        while (!validIdEntered)
        {
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                if (exerciseRepository.TryGetExerciseById(result, out _) || result == -1)
                {
                    return result;
                }
            }
            Console.Write("\nThis is not a valid id, please enter a number or to return to main menu type '-1': ");
        }
        return -1;
    }
}

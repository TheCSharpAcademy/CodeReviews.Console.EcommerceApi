using ExerciseTrackerUI.Models;

namespace ExerciseTrackerUI;

public class UserInput
{
    public async Task Menu()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("Shift menu\n");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Press '0' to exit the application");
            Console.WriteLine("Press '1' to view all Exercises");
            Console.WriteLine("Press '2' to add a exercise");
            Console.WriteLine("Press '3' to update a exercise");
            Console.WriteLine("Press '4' to delete a exercise");
            Console.WriteLine("----------------------------------");
            var input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    isRunning = false;
                    Environment.Exit(0);
                    break;

                case "1":
                    await ShowExercises();
                    break;

                case "2":
                    await AddExercise();
                    break;

                case "3":
                    await UpdateExercise();
                    break;

                case "4":

                    await DeleteExercise();
                    break;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    public async Task ShowExercises()
    {
        var exercises = await ExerciseHtpp.GetAllExercises();
        if (exercises.Any())
        {
            foreach (var exercise in exercises)
            {
                Console.WriteLine($"{exercise.Id}. Name:{exercise.ExerciseName} || Start Date:{exercise.StartTime.ToString("MM-dd-yyyy HH:mm:ss")} || End Date:{exercise.EndTime.ToString("MM-dd-yyyy HH:mm:ss")} || Duration:{exercise.Duration.ToString(@"hh\:mm\:ss")} || Comments:{exercise.Comments}\n");
            }
        }
        else Console.WriteLine("No exercise found");
    }

    public async Task AddExercise()

    {
        Console.Clear();
        var exercise = new Exercise();
        Console.WriteLine("Please enter the exercise name or type 0  to go back to menu");
        var name = Console.ReadLine();
        if (name == "0") return;
        while (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Input can not be empty try again.");
            name = Console.ReadLine();
        }
        Console.WriteLine("Please enter the start date in the HH:mm format");
        var startDate = Console.ReadLine();
        while (!DateTime.TryParseExact(startDate, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _) || string.IsNullOrEmpty(startDate))
        {
            Console.WriteLine("Invalid date time");
            startDate = Console.ReadLine();
        }

        Console.WriteLine("Please enter the end date in the HH:mm format");
        var endDate = Console.ReadLine();
        while (string.IsNullOrEmpty(endDate) ||
               !DateTime.TryParseExact(endDate, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime endTime) ||
               DateTime.Parse(startDate) >= endTime)
        {
            Console.WriteLine("End time can't be earlier than start time and must be in HH:mm format. Try again.");
            endDate = Console.ReadLine();
        }

        Console.WriteLine("Add a comment to your workout or press any key to skip ");
        var comment = Console.ReadLine();

        exercise.ExerciseName = name;
        exercise.StartTime = DateTime.Parse(startDate);
        exercise.EndTime = DateTime.Parse(endDate);
        exercise.Duration = DateTime.Parse(endDate) - DateTime.Parse(startDate);
        exercise.Comments = comment;
        await ExerciseHtpp.AddExercise(exercise);
    }

    public async Task UpdateExercise()

    {
        Console.Clear();
        var exercises = await ExerciseHtpp.GetAllExercises();
        await ShowExercises();
        var exercise = new Exercise();
        Console.WriteLine("Please enter the exercise number you want to update or type 0  to go back to menu");
        var inputId = Console.ReadLine();
        if (!int.TryParse(inputId, out int exerciseId) ||
               !exercises.Any(e => e.Id == exerciseId))
        {
            Console.WriteLine("Exercise does not exist");
            return;
        }
        while (!int.TryParse(inputId, out _))
        {
            Console.WriteLine("Invalid exercise ID. Please enter a valid number.");

            inputId = Console.ReadLine();
        }
        Console.WriteLine("Please enter the updated exercise  you want to update or type 0  to go back to menu");
        var name = Console.ReadLine();

        while (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Input can not be empty try again.");
            name = Console.ReadLine();
        }

        if (name == "0") return;
        while (string.IsNullOrEmpty(name)) Console.WriteLine("Input can not be empty try again.");

        Console.WriteLine("Please enter the updated start date in the HH:mm format");
        var startDate = Console.ReadLine();
        while (!DateTime.TryParseExact(startDate, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _) || string.IsNullOrEmpty(startDate))
        {
            Console.WriteLine("Invalid date time");
            startDate = Console.ReadLine();
        }

        Console.WriteLine("Please enter the updated end date in the HH:mm format");
        var endDate = Console.ReadLine();

        while (string.IsNullOrEmpty(endDate) ||
                !DateTime.TryParseExact(endDate, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime endTime) ||
                DateTime.Parse(startDate) >= endTime)
        {
            Console.WriteLine("End time can't be earlier than start time and must be in HH:mm format. Try again.");
            endDate = Console.ReadLine();
        }
        while (!DateTime.TryParseExact(endDate, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _) || string.IsNullOrEmpty(startDate))
        {
            Console.WriteLine("Invalid date time");
            endDate = Console.ReadLine();
        }

        Console.WriteLine("Add a comment to your workout or press any key to skip ");
        var comment = Console.ReadLine();

        exercise.ExerciseName = name;
        exercise.StartTime = DateTime.Parse(startDate);
        exercise.EndTime = DateTime.Parse(endDate);
        exercise.Duration = DateTime.Parse(endDate) - DateTime.Parse(startDate);
        exercise.Comments = comment;
        await ExerciseHtpp.UpdateExercise(Convert.ToInt32(exerciseId), exercise);
    }

    private async Task DeleteExercise()
    {
        Console.Clear();
        await ShowExercises();
        var exercises = await ExerciseHtpp.GetAllExercises();
        Console.WriteLine("Please enter the number you want to delete");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out int exerciseId) ||
            !exercises.Any(e => e.Id == exerciseId))
        {
            Console.WriteLine("Exercise does not exist");
            return;
        }
        await ExerciseHtpp.DeleteExercise(exerciseId);
    }
}
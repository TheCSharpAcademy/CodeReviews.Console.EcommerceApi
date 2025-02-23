using ExerciseTracker.Models;
using ExerciseTracker.Service;
using ExerciseTracker.View;
using Spectre.Console;

namespace ExerciseTracker.Controller;

internal class ExerciseController
{
    internal static void AddExercise()
    {
        ExerciseService service = new ExerciseService();
        Exercise exercise = new Exercise();
        bool datesValid = false;

        while (!datesValid)
        {
            Helpers.PrintHeader();

            Console.WriteLine("Enter the start date and time of the exercise (MM-dd-yy HH:mm)");
            exercise.DateStart = Helpers.GetDateTime();

            Console.WriteLine("Enter the end date and time of the exercise (MM-dd-yy HH:mm)");
            exercise.DateEnd = Helpers.GetDateTime();

            datesValid = Helpers.ValidateDates(exercise.DateStart, exercise.DateEnd);

        }

        exercise.Duration = exercise.DateEnd - exercise.DateStart;

        exercise.Comment = Helpers.GetComment();

        service.AddExercise(exercise);

        AnsiConsole.Markup("[green]Exercise added successfully.[/]");
        AnsiConsole.Markup("[green]Press any key to return to the main menu[/]");
        Console.ReadKey();
    }

    internal static void DeleteExercise()
    {
        ExerciseService service = new ExerciseService();
        bool idValid = false;

        List<Exercise> exercises = service.GetExercises();

        if (exercises != null)
        {
            UserInterface.PrintExercises(exercises);
            int id = AnsiConsole.Ask<int>("Enter the ID of the exercise you would like to delete: ");
            idValid = Helpers.ValidateID(exercises, id);

            if (idValid)
            {
                service.DeleteExercise(id);
                AnsiConsole.Markup("[green]Exercise deleted successfully.[/]");
                AnsiConsole.Markup("[green]Press any key to return to the main menu[/]");
                Console.ReadKey();
            }

        }
        else
        {
            AnsiConsole.Markup("[red]No exercises found.[/]");
            AnsiConsole.Markup("[red]Press any key to continue.[/]");
            Console.ReadKey();
        }
    }

    internal static void UpdateExercise()
    {
        ExerciseService service = new ExerciseService();
        Exercise exercise = new Exercise();
        bool idValid = false;

        List<Exercise> exercises = service.GetExercises();

        if (exercises != null)
        {
            UserInterface.PrintExercises(exercises);
            int id = AnsiConsole.Ask<int>("Enter the ID of the exercise you would like to update: ");
            idValid = Helpers.ValidateID(exercises, id);

            if (idValid)
            {
                bool datesValid = false;

                while (!datesValid)
                {
                    Console.WriteLine("Enter the start date and time of the exercise (MM-dd-yy HH:mm)");
                    exercise.DateStart = Helpers.GetDateTime();

                    Console.WriteLine("Enter the end date and time of the exercise (MM-dd-yy HH:mm)");
                    exercise.DateEnd = Helpers.GetDateTime();

                    datesValid = Helpers.ValidateDates(exercise.DateStart, exercise.DateEnd);

                }

                exercise.Duration = exercise.DateEnd - exercise.DateStart;

                exercise.Comment = Helpers.GetComment();

                service.UpdateExercise(id, exercise);

                AnsiConsole.Markup("[green]Exercise updated successfully.[/]");
                AnsiConsole.Markup("[green]Press any key to return to the main menu[/]");
                Console.ReadKey();
            }

        }
        else
        {
            AnsiConsole.Markup("[red]No exercises found.[/]");
            AnsiConsole.Markup("[red]Press any key to continue.[/]");
            Console.ReadKey();
        }
    }

    internal static void ViewExercises()
    {
        ExerciseService service = new ExerciseService();

        List<Exercise> exercises = service.GetExercises();

        if (exercises != null)
        { 
            UserInterface.PrintExercises(exercises);
            AnsiConsole.Markup("[blue]Press any key to return to the main menu[/]");
            Console.ReadKey();
        }
        else
        {
            AnsiConsole.Markup("[red]No exercises found.[/]");
            AnsiConsole.Markup("[red]Press any key to continue.[/]");
            Console.ReadKey();
        }
    }
}

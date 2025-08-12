using ExerciseTracker.Cactus.Model;
using ExerciseTracker.Cactus.Util;
using Spectre.Console;

namespace ExerciseTracker.Cactus.Service
{
    public static class ExerciseServiceHelper
    {
        public static Exercise InputExercise()
        {
            Console.WriteLine("Please type start date");
            DateTime startDate = GetValidDate();

            Console.WriteLine("Please type end date");
            DateTime endDate = GetValidEndDate(startDate);

            int duration = AnsiConsole.Ask<int>("Duration:");

            string comments = AnsiConsole.Ask<string>("Comments:");

            Exercise exercise = new Exercise(startDate, endDate, duration, comments);

            return exercise;
        }


        public static DateTime GetValidDate()
        {
            var dateStr = AnsiConsole.Ask<string>("Date (format: yyyy-MM-dd):");
            DateTime date;
            while (!Validator.IsValidDate(dateStr, out date))
            {
                Console.WriteLine("Please type correct format date.");
                dateStr = AnsiConsole.Ask<string>("Date (format: yyyy-MM-dd):");
            }

            return date;
        }


        public static DateTime GetValidEndDate(DateTime startDate)
        {
            DateTime endDate = GetValidDate();
            while (endDate < startDate)
            {
                Console.WriteLine($"End date should late than start date {startDate}.");
                var endDateStr = AnsiConsole.Ask<string>("End date (format: yyyy-MM-dd): ");
                while (!Validator.IsValidDate(endDateStr, out endDate))
                {
                    Console.WriteLine("Please type correct format date.");
                    endDateStr = AnsiConsole.Ask<string>("End date (format: yyyy-MM-dd): ");
                }
            }
            return endDate;
        }


        public static Exercise SelectExerciseById(IEnumerable<Exercise> exercise)
        {
            List<int> uniqueIds = exercise.Select(exercise => exercise.Id).Distinct().ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Please choose the exercise you like to operate?")
                    .AddChoices(uniqueIds));

            var selectedExercise = exercise.Where(exercise => exercise.Id == selectedId).ToArray()[0];

            return selectedExercise;
        }
    }
}

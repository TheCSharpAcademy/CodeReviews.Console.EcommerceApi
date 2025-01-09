using ExerciseTracker.KroksasC.Models;
using Spectre.Console;
using ExerciseTracker.KroksasC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExerciseTracker.KroksasC.Repositaries;
using ExerciseTracker.KroksasC.Controllers;

namespace ExerciseTracker.KroksasC.UI
{
    public class UserInput
    {
        private readonly ValidationService ValidationService = new ValidationService();

        private readonly ExerciseService exerciseService;

        // Inject ExerciseService into UserInput
        public UserInput(ExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }
        public async Task<Exercise> GetDeleteInput()
        {
            var exercise = await GetExerciseById("delete");
            if (exercise == null)
            {
                return null;
            }
            return exercise;
        }
        public async Task<Exercise> GetUpdateInput()
        {
            var Exercise = await GetExerciseById("update");

            if(Exercise == null)
            {
                return null;
            }

            var options = new[] {"Update Start Time", "Update End Time", "Update Comment" };

            // Display a checklist for multiple selection
            var selectedOptions = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Choose your [green]options[/]:")
                    .PageSize(10) // Number of items to display at once
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to toggle an option, [green]<enter>[/] to accept)[/]")
                    .AddChoices(options)
            );

            if (selectedOptions.Contains("Update Comment"))
            {
                Console.WriteLine("Enter new name:");
                string? name = Console.ReadLine();
            }

            if(selectedOptions.Contains("Update Start Time"))
            {
                Console.WriteLine("Enter new start time for Exercise(format:(yyyy-MM-dd HH:mm:ss))");
                string? start = Console.ReadLine();
                Exercise.StartTime = ValidationService.ValidateDate(start);
            }

            if (selectedOptions.Contains("Update End Time"))
            {
                Console.WriteLine("Enter new end time for Exercise(format:(yyyy-MM-dd HH:mm:ss))");
                string? end = Console.ReadLine();
                Exercise.EndTime = ValidationService.ValidateDate(end);
            }



            if(selectedOptions.Count == 0)
            {
                Console.WriteLine("No options were choosed. Press any key to return");
                Console.ReadLine();
            }
            return Exercise;

        }
        public Exercise GetCreateInput()
        {
            Console.WriteLine("What is the comment of the Exercise?");
            string? comment = Console.ReadLine();

            Console.WriteLine("What is the start time of the Exercise?(format:(yyyy-MM-dd HH:mm:ss))");
            string? start = Console.ReadLine();
            DateTime startTime = ValidationService.ValidateDate(start);

            Console.WriteLine("What is the end time of the Exercise?(format:(yyyy-MM-dd HH:mm:ss))");
            string? end = Console.ReadLine();
            DateTime endTime = ValidationService.ValidateDate(end);

            return new Exercise { EndTime = endTime, StartTime = startTime, Comment = comment };
        }
        public static async Task ShowExercises(IEnumerable<Exercise> Exercises)
        {
            Table table = new Table();
            table.AddColumns("Id", "Start Exercise", "End Exercise", "Duration", "Comment");
            foreach (Exercise Exercise in Exercises) 
            {
                table.AddRow(Exercise.Id.ToString(), Exercise.StartTime.ToString(), Exercise.EndTime.ToString(), Exercise.Duration.ToString(), Exercise.Comment);
            }
            AnsiConsole.Render(table);
            Console.WriteLine("Enter any key to continue");
            Console.ReadLine();
        }
        public async Task<Exercise> GetExerciseById(string action)
        {
            await exerciseService.ShowAllExercises();

            var option = AnsiConsole.Ask<string?>($"Enter Exercise id that you want to choose to {action}");

            int choosedId = ValidationService.ValidateNumber(option);

            var exercise = await exerciseService.GetExerciseById(choosedId);

            if (exercise == null)
            {
                Console.WriteLine("Id that you choose doesn't exist! Press any key to return");
                Console.ReadLine();
                return null;
            }
            else
            {
                return exercise;
            }
        }
    }
}

using System.Globalization;
using ExerciseTracker.UgniusFalze.Models;
using Spectre.Console;

namespace ExerciseTracker.UgniusFalze.Utils;


public static class Display
{
    public static void DisplayExercises(List<Pullup> exercises)
    {
        if (exercises.Count == 0)
        {
            EmptyExercises();
            Continue();
            return;
        }
        var table = new Table();
        table.Title("Your current exercises");
        table.AddColumns("Start Date", "End date", "Comment", "Duration in minutes", "Repetitions");
        foreach (var exercise in exercises)
        {
            table.AddRow(exercise.DateStart.ToString(CultureInfo.CurrentCulture),
                exercise.DateEnd.ToString(CultureInfo.CurrentCulture), exercise.Comment, exercise.Duration.TotalMinutes.ToString(CultureInfo.CurrentCulture), exercise.Repetitions.ToString());
        }

        AnsiConsole.Write(table);
        Continue();
    }

    public static void Continue()
    {
        Console.WriteLine("Press any key to continue..");
        Console.ReadKey();
    }

    public static string InvalidDate()
    {
        return "Your entered date is invalid, please use mm/dd/yyyy format";
    }

    public static void FailedToInsert()
    {
        Console.WriteLine("Failed to insert new exercise");
    }

    public static void FailedToUpdate()
    {
        Console.WriteLine("Failed to update exercise");
    }

    public static void FailedToDelete()
    {
        Console.WriteLine("Failed to delete exercise");
    }

    public static void EmptyExercises()
    {
        Console.WriteLine("There are currently no exercises");
    }

    public static void UpdateSucceeded()
    {
        Console.WriteLine("Exercise updated successfully");
    }

    public static void InsertSucceeded()
    {
        Console.WriteLine("Exercise inserted successfully");
    }
    
}
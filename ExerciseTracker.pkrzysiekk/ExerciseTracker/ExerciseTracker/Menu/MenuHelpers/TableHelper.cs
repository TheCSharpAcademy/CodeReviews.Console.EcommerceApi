using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker.Menu.MenuHelpers;

public static class TableHelper
{
    public static void Show(IEnumerable<Exercise> exercises)
    {
        var table = new Table();
        
        table.AddColumn("Description");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("ID");
        foreach (var exercise in exercises)
        {
            table.AddRow(exercise.Description,
                exercise.DateStart.ToString("yyyy/MM/dd HH:mm:ss UTC")
                ,exercise.DateEnd.ToString("yyyy/MM/dd HH:mm:ss UTC"),exercise.Id.ToString());
     
            
        } 
        AnsiConsole.Write(table);
    }
    
}
using System.ComponentModel.DataAnnotations;

namespace ExerciseTrackerCLI.Models;

public class TreadmillRun : IExercise
{
    public int Id { get; set; }
    
    public DateTime DateStart { get; set; }
    
    public DateTime DateEnd { get; set; }

    public TimeSpan Duration => DateEnd - DateStart;

    public string Comments { get; set; } = "";

    public string ListDisplay() => $"{DateStart:d}: {Duration.TotalHours:f} hrs";

    public string? LongDisplay() => $"{DateStart:G} --- {DateEnd:G}\n{Duration.TotalHours:f} hrs\nComments: {Comments}\n";
}
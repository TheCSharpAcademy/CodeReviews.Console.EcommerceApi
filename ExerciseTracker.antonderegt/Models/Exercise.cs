using static ExerciseTracker.Models.Enums;

namespace ExerciseTracker.Models;

public class Exercise
{
    public int Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration => DateEnd - DateStart;
    public string Comments { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
}
namespace ExerciseTrackerAPI.Models;

public class Exercise
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration => DateEnd - DateStart;
    public int Repetitions { get; set; }
    public int CustomerId { get; set; }
    public string Comments { get; set; } = string.Empty;
}

namespace ExerciseTracker.tonyissa.Models;

public class ExerciseSession
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Comments { get; set; }
    public TimeSpan Duration { get; set; }
}
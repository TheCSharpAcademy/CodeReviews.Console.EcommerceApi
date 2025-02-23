namespace ExerciseTracker.Models;

internal class Exercise
{
    public int Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration { get; set; }
    public string Comment { get; set; } = String.Empty;
}


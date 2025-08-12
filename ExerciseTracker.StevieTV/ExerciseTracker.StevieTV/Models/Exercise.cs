namespace ExerciseTracker.StevieTV.Models;

public class Exercise
{
    public long Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration { get; set; }
    public string Comment { get; set; }
}
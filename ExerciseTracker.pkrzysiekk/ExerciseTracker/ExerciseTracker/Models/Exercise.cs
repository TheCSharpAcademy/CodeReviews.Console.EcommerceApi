namespace ExerciseTracker.Models;

public class Exercise
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
}
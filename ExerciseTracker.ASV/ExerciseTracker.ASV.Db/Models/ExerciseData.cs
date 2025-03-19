namespace ExerciseTracker.ASV.Db.Models;

public class ExerciseData
{
    public int Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Description { get; set; }
}
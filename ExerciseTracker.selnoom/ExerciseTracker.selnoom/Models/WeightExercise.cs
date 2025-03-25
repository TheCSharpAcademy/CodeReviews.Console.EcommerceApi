namespace ExerciseTracker.selnoom.Models;

public class WeightExercise
{
    public int Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public int Sets { get; set; }
    public int Repetitions { get; set; }
}

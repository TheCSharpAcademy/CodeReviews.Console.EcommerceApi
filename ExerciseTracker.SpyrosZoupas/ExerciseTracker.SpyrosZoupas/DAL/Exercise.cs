namespace ExerciseTracker.SpyrosZoupas.DAL;

public class Exercise
{
    public int ExerciseId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration
    {
        get => DateEnd.TimeOfDay - DateStart.TimeOfDay;
    }
    public string Comments { get; set; }
}
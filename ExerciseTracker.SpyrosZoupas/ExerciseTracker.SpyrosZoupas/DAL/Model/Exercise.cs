namespace ExerciseTracker.SpyrosZoupas.DAL.Model;

public class Exercise
{
    public int Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration
    {
        get => DateEnd - DateStart;
    }
    public string Comments { get; set; }
}

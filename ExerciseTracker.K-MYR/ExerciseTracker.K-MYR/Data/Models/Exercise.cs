namespace ExerciseTracker.K_MYR;

public class Exercise
{
    public int Id { get; set; }
    public string Type { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public long Duration { get; set; }
    public string Comments { get; set; } = "";
}

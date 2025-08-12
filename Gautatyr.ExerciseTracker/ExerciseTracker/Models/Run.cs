namespace ExerciseTracker.Models;

public class Run
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Distance { get; set; }
    public string? Comment { get; set; }

    public void SetDuration()
    {
        Duration = End - Start;
    }
}
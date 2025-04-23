namespace ExerciseTracker.Models;
public class Pushup
{
    public int Id { get; set; }
    public DateTime Date { get; set; }  // Just one date, no start/end
    public int Reps { get; set; }       // Number of push-ups
    public string? Comments { get; set; }
}

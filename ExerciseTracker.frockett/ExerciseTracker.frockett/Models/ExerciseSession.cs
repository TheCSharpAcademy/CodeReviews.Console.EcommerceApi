
namespace ExerciseTracker.frockett.Models;

public class ExerciseSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;
    public string? Comments { get; set; }
}

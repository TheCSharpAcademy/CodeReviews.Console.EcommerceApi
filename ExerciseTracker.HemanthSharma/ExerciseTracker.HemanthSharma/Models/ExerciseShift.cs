using ExerciseTracker.HemanthSharma.Interfaces;

namespace ExerciseTracker.Study.Models;

public class ExerciseShift : IEntity<ExerciseShift>
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime ExerciseDate { get; set; }
    public int Duration => (int)(EndTime - StartTime).TotalMinutes;
    public string? Comments { get; set; }
    public Exercise Exercise { get; set; }
}

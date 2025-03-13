namespace ExerciseTracker.Core.DTOs;

public class ExerciseDTO
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Duration => EndDate - StartDate;
    public string? Comments { get; set; }
}

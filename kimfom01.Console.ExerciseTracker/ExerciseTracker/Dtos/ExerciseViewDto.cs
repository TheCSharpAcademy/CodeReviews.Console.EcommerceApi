namespace ExerciseTracker.Dtos;

public class ExerciseViewDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Duration { get; set; }
    public string? Comments { get; set; }
}
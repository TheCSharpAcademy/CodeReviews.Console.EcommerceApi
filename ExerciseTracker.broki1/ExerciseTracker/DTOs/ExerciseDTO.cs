namespace ExerciseTracker.DTOs;

internal class ExerciseDTO
{
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public TimeSpan Duration { get; set; }
    public string Comments { get; set; }
}

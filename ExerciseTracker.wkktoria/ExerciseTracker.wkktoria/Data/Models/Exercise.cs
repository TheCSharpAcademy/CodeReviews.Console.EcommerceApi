namespace ExerciseTracker.wkktoria.Data.Models;

public class Exercise
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public TimeSpan Duration { get; set; }

    public string? Comment { get; set; }

    public override string ToString()
    {
        return $"Start Date: {StartDate:f} - End Date: {EndDate:f}";
    }
}
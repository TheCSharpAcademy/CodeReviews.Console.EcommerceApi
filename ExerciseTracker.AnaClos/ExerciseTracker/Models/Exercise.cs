using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Models;

public class Exercise
{
    public int Id { get; set; }
    [Required]
    public DateTime DateStart { get; set; }
    [Required]
    public DateTime DateEnd { get; set; }
    [Required]
    public TimeSpan Duration { get; set; }
    public string? Comments { get; set; }

    public void CalculateDuration()
    {
        Duration = DateEnd - DateStart;
    }
}
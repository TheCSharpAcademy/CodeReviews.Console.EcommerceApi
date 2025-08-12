using Microsoft.EntityFrameworkCore;
namespace ExerciseTracker.kjanos89.Models;

public class Exercise
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration { get; set; }
    public string Comments { get; set; }
}
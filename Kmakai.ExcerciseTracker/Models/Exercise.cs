

using System.ComponentModel.DataAnnotations;

namespace Kmakai.ExerciseTracker.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration
    {
        get { return DateEnd - DateStart; }
    }
    public string? Comments { get; set; }
}

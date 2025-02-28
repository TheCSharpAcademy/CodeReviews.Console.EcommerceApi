using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.cacheMe512;

internal class Exercise
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime DateStart { get; set; }

    [Required]
    public DateTime DateEnd { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    public int Sets { get; set; }

    public int Reps { get; set; }

    public double Weight { get; set; }
}

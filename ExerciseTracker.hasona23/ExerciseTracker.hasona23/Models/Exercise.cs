using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExerciseTracker.hasona23.Models;

public class Exercise
{
    [Key,Required]
    public int Id { get; set; }
    [MaxLength(255)]
    //type - comments - etc
    public string? Description { get; set; }
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    [NotMapped]
    public TimeSpan Duration => End - Start;
    public Exercise(){}

    public Exercise(ExerciseCreate exercise)
    {
        Start = exercise.Start;
        End = exercise.End;
        Description = exercise.Description;
    }

    public Exercise(int id, string description, DateTime start, DateTime end)
    {
        Id = id;
        Description = description;
        Start = start;
        End = end;
    }

    public override string ToString() => $"Id: {Id} | Description: {Description} | Start: {Start} | End: {End}";
}

public record ExerciseCreate(string? Description, DateTime Start, DateTime End);
public record ExerciseUpdate(int Id,string? Description, DateTime Start, DateTime End);
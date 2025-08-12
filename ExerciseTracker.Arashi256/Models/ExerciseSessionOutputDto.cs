using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Arashi256.Models
{
    public record ExerciseSessionOutputDto
    {
        [Required]
        public int DisplayId { get; set; }
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(25)]
        public string Type { get; set; } = string.Empty;
        [Required]
        public DateTime DateTimeStart { get; set; }
        [Required]
        public DateTime DateTimeEnd { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [MaxLength(255)]
        public string? Comments { get; set; }
    }
}

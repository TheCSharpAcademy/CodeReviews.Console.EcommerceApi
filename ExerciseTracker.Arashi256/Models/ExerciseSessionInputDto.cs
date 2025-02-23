using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Arashi256.Models
{
    public record ExerciseSessionInputDto
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(25)]
        public string Type { get; set; } = string.Empty;
        [Required]
        public DateTime DateTimeStart { get; set; }
        [Required]
        public DateTime DateTimeEnd { get; set; }
        [MaxLength(255)]
        public string? Comments { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Exercisetacker.Entities;

public class Jogging
{
    public int Id { get; set; }
    [Required]
    public DateTime DateStart { get; set; }
    [Required]
    public DateTime DateEnd {get;set;}
    [Required]
    public TimeSpan Duration {get;set;}
    [Required]
    [MaxLength(255)]
    public string Comments {get;set;}
}
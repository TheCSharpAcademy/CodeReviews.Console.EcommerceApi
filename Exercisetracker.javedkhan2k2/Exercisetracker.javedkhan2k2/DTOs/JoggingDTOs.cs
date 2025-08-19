using System.ComponentModel.DataAnnotations;
using Exercisetacker.Entities;

namespace Exercisetacker.DTOs;

public class JoggingAddDto
{
    public DateTime DateStart { get; set; }
    public DateTime DateEnd {get;set;}
    public TimeSpan Duration {get;set;}
    public string Comments {get;set;}

    public Jogging ToJogging()
    {
        return new Jogging
        {
            DateStart = this.DateStart,
            DateEnd = this.DateEnd,
            Duration = this.Duration,
            Comments = this.Comments
        };
    }

}
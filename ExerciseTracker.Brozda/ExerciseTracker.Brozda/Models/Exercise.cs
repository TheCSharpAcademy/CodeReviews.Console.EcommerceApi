

namespace ExerciseTracker.Brozda.Models
{
    /// <summary>
    /// Represents record in Excercises table
    /// </summary>
    internal class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TypeId { get; set; }
        public ExerciseType Type { get; set; } = null!;
        public double Volume { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public long? Duration { get; set; }
        public string? Comments { get; set; }

        /// <summary>
        /// Maps value from passed value to current one
        /// </summary>
        /// <param name="updated"></param>
        public void MapFromUpdate(Exercise updated)
        {
            this.Name = updated.Name;
            this.Volume = updated.Volume;
            this.DateStart = updated.DateStart;
            this.DateEnd = updated.DateEnd;
            this.Duration = updated.Duration;
            this.Comments = updated.Comments;
        }

        public static Exercise MapFromDto(ExerciseDto dto)
        {
            return new Exercise
            {
                Id = dto.Id,
                Name = dto.Name,
                TypeId = dto.TypeId,
                Volume = dto.Volume,
                DateStart = dto.DateStart,
                DateEnd = dto.DateEnd,
                Duration = dto.Duration,
                Comments = dto.Comments,
            };
        }
        public ExerciseDto MapToDto()
        {
            return new ExerciseDto
            {
                Id = this.Id,
                Name = this.Name,
                TypeId = this.TypeId,
                TypeName = this.Type.Name,
                Unit = this.Type.Unit,
                Volume = this.Volume,
                DateStart = this.DateStart,
                DateEnd = this.DateEnd,
                Duration = this.Duration,
                Comments = this.Comments,
            };
        }
    }
    
}



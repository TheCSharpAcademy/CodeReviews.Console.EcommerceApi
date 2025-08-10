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

        /// <summary>
        /// Maps values from database model to DTO
        /// </summary>
        /// <returns>Mapped <see cref="ExerciseDto"/> object</returns>
        public static ExerciseDto MapToDto(Exercise model)
        {
            return new ExerciseDto
            {
                Id = model.Id,
                Name = model.Name,
                TypeId = model.TypeId,
                TypeName = model.Type.Name,
                Unit = model.Type.Unit,
                Volume = model.Volume,
                DateStart = model.DateStart,
                DateEnd = model.DateEnd,
                Duration = model.Duration,
                Comments = model.Comments,
            };
        }

        /// <summary>
        /// Maps DTO to valid database model
        /// </summary>
        /// <returns>Mapped <see cref="Exercise"/> object </returns>
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
    }
}
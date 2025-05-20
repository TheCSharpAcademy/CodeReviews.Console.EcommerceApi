namespace ExerciseTracker.Brozda.Models
{
    internal class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public double Volume { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public long? Duration { get; set; }
        public string? Comments { get; set; }

        public static ExerciseDto MapToDto(Exercise ex)
        {
            return new ExerciseDto
            {
                Id = ex.Id,
                Name = ex.Name,
                TypeId = ex.TypeId,
                TypeName = ex.Type.Name,
                Unit = ex.Type.Unit,
                Volume = ex.Volume,
                DateStart = ex.DateStart,
                DateEnd = ex.DateEnd,
                Duration = ex.Duration,
                Comments = ex.Comments,
            };
        }
        public Exercise MapFromDto()
        {
            return new Exercise
            {
                Id = this.Id,
                Name = this.Name,
                TypeId = this.TypeId,
                Volume = this.Volume,
                DateStart = this.DateStart,
                DateEnd = this.DateEnd,
                Duration = this.Duration,
                Comments = this.Comments,
            };
        }
    }

    
}

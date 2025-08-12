namespace ExerciseTracker.JsPeanut
{
    public class ExerciseRepository : IExerciseRepository
    {
        private ExerciseContext context;

        public ExerciseRepository(ExerciseContext context)
        {
            this.context = context;
        }

        public IEnumerable<Exercise> GetAll()
        {
            try
            {
                return context.Exercises.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't get the list of exercise sessions: {ex.Message}");
            }
        }

        public Exercise Get(int id)
        {
            try
            {
                return context.Exercises.Find(id);
            }
            catch (Exception ex) when (context.Exercises.Find(id) != null)
            {
                throw new Exception($"Couldn't get the exercise session: {ex.Message}");
            }
        }

        public void Insert(Exercise exercise)
        {
            try
            {
                context.Exercises.Add(exercise);

                Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add the exercise session: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            Exercise exercise = context.Exercises.Find(id);
            try
            {
                context.Exercises.Remove(exercise);

                Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete the exercise session: {ex.Message}");
            }
        }

        public void Update(Exercise exercise)
        {
            try
            {
                context.Entry(exercise).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update the exercise session: {ex.Message}");
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

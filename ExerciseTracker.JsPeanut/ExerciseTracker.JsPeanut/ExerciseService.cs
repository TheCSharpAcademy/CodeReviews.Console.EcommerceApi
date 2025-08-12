namespace ExerciseTracker.JsPeanut
{
    public class ExerciseService : IExerciseService
    {
        private IExerciseRepository exerciseRepository;

        public ExerciseService()
        {
            exerciseRepository = new ExerciseRepository(new ExerciseContext());
        }

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public IEnumerable<Exercise> GetExercises()
        {
            return exerciseRepository.GetAll();
        }

        public Exercise Get(int id)
        {
            return exerciseRepository.Get(id);
        }

        public void Insert(Exercise exercise)
        {
            exerciseRepository.Insert(exercise);
        }

        public void Delete(int id)
        {
            exerciseRepository.Delete(id);
        }

        public void Update(Exercise exercise)
        {
            exerciseRepository.Update(exercise);
        }

        public void Save()
        {
            exerciseRepository.Save();
        }
    }
}

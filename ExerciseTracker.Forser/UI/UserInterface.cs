namespace ExerciseTracker.Forser.UI
{
    internal class UserInterface : IUserInterface
    {
        private readonly IExerciseReposoitory _exerciseReposoitory;
        public UserInterface(IExerciseReposoitory exerciseReposoitory)
        {
            _exerciseReposoitory = exerciseReposoitory;
        }
        public DateTime GetEndTime(DateTime startTime)
        {
            throw new NotImplementedException();
        }
        public int GetExerciseId(int id)
        {
            bool result = _exerciseReposoitory.GetExerciseById(id, out _);

            if(result)
            {
                return id;
            }
            return -1;
        }
        public DateTime GetStartTime()
        {
            throw new NotImplementedException();
        }
        public static void DisplayExerciseTable(List<Exercise> exercises)
        {
            Table table = new Table();
            table.Expand();
            table.AddColumns("Id","Date Started","Date Ended","Duration","Comments");

            foreach (Exercise exercise in exercises)
            {
                table.AddRow($"{exercise.Id}", $"{exercise.DateStart}", $"{exercise.DateEnd}",$"{exercise.Duration}", $"{exercise.Comments}");
            }

            AnsiConsole.Write(table);
        }
    }
}
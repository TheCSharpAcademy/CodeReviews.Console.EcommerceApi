using ExerciseTracker.KroksasC.Models;
using ExerciseTracker.KroksasC.Repositaries;
using ExerciseTracker.KroksasC.UI;

namespace ExerciseTracker.KroksasC.Services
{
    public class ExerciseService
    {
        public readonly IUnitOfWork unitOfWork;

        public ExerciseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task AddExercise()
        {
            var userInput = new UserInput(this);
            var exercise = userInput.GetCreateInput();
            await unitOfWork.Exercises.Add(exercise);
            await unitOfWork.CompleteAsync();
        }
        public async Task<Exercise> GetExerciseById(int id)
        {
            var ex = await unitOfWork.Exercises.GetById(id);
            return ex;
        }
        public async Task<IEnumerable<Exercise>> GetAllExercises()
        {
            return await unitOfWork.Exercises.GetAll();
        }
        public async Task DeleteExercise()
        {
            var userInput = new UserInput(this);
            var exercise = await userInput.GetDeleteInput();
            if (exercise == null)
            {
                return;
            }
            unitOfWork.Exercises.Delete(exercise);
            await unitOfWork.CompleteAsync();
        }
        public async Task UpdateExercise()
        {
            var userInput = new UserInput(this);
            var exercise = await userInput.GetUpdateInput();
            if (exercise == null) 
            {
                return;
            }
            unitOfWork.Exercises.Update(exercise);
            await unitOfWork.CompleteAsync();
        }
        public async Task ShowAllExercises()
        {
            await UserInput.ShowExercises(await GetAllExercises());
        }
    }
}

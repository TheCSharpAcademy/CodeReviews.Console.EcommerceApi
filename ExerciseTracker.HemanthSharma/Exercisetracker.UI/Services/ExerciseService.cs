using ExerciseTracker.UI.Models;
using ExerciseTracker.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTracker.UI.Services
{
    public class ExerciseService
    {
        private static readonly Repository<Exercise> Repo= new Repository<Exercise>();
        public static void  CreateExercise()
        {
            Exercise NewExercise = UserInputs<ExerciseDto>.GetNewExercise();
            ResponseDto<Exercise> CreatedExercise= Repo.CreateEntity(NewExercise).GetAwaiter().GetResult();
            UserOutputs<Exercise>.ShowResponse(CreatedExercise);
        }

        public static void DeleteExercise()
        {
            List<Exercise> Exercises = Repo.GetAllEntities().GetAwaiter().GetResult().Data;
            int? UserChoice = UserInputs<Exercise>.GetExerciseById(Exercises);
            ResponseDto<Exercise> DeleteResponse = Repo.DeleteEntity(UserChoice).GetAwaiter().GetResult();
            UserOutputs<Exercise>.ShowResponse(DeleteResponse);
        }

        public static void GetAllExercises()
        {
            ResponseDto<Exercise> Response =  Repo.GetAllEntities().GetAwaiter().GetResult();
            UserOutputs<Exercise>.ShowResponse(Response);
        }

        public static void GetSingleExercise()
        {
            List<Exercise> Exercises= Repo.GetAllEntities().GetAwaiter().GetResult().Data;
            int? UserChoice=UserInputs<Exercise>.GetExerciseById(Exercises);
            ResponseDto<Exercise> Response = Repo.GetEntiryById(UserChoice).GetAwaiter().GetResult();
            UserOutputs<Exercise>.ShowResponse(Response);
        }

        public static void UpdateExercise()
        {
            List<Exercise> Exercises = Repo.GetAllEntities().GetAwaiter().GetResult().Data;
            int? UserChoice = UserInputs<Exercise>.GetExerciseById(Exercises);
            ResponseDto<Exercise> Response = Repo.GetEntiryById(UserChoice).GetAwaiter().GetResult();
            Exercise UpdatedExercise = UserInputs<Exercise>.GetUpdatedEntity(Response.Data);
            ResponseDto<Exercise> DeleteResponse = Repo.UpdateEntity(UpdatedExercise, UserChoice).GetAwaiter().GetResult();
            UserOutputs<Exercise>.ShowResponse(Response);
        }
    }
}

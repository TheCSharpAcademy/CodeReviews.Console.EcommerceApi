using ExerciseTracker.UI.Models;
using ExerciseTracker.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTracker.UI.Services
{
    public static class ShiftService
    {
        public static readonly Repository<ExerciseShiftDto> Repo = new Repository<ExerciseShiftDto>();
        public static readonly Repository<Exercise> ExerciseRepo = new Repository<Exercise>();
        public static List<Exercise> GetAvailableExercises()
        {
            ResponseDto<Exercise> ExercisesResponse = ExerciseRepo.GetAllEntities().GetAwaiter().GetResult();
            return ExercisesResponse.Data;
        }
            
        public static void CreateShift()
        {
            List<Exercise> AvailableExercises = GetAvailableExercises();
            ExerciseShiftDto NewExercise = UserInputs<ExerciseShift>.GetNewShift(AvailableExercises);
            ResponseDto<ExerciseShiftDto> CreateResponse=Repo.CreateEntity(NewExercise).GetAwaiter().GetResult();
            UserOutputs<ExerciseShiftDto>.ShowResponse(CreateResponse);
        }

        public static void DeleteShift()
        {
            ResponseDto<ExerciseShiftDto> ShiftsList = Repo.GetAllEntities().GetAwaiter().GetResult();
            int SelectedShiftId = UserInputs<ExerciseShift>.GetShiftById(ShiftsList.Data);
            if(SelectedShiftId==-1)
            {
                return;
            }
            ResponseDto<ExerciseShiftDto> DeleteShiftRsponse = Repo.DeleteEntity(SelectedShiftId).GetAwaiter().GetResult();
            UserOutputs<ExerciseShiftDto>.ShowResponse(DeleteShiftRsponse);
        }

        public static void GetAllShifts()
        {          
            ResponseDto<ExerciseShiftDto> ShiftList=Repo.GetAllEntities().GetAwaiter().GetResult();
            UserOutputs<ExerciseShiftDto>.ShowResponse(ShiftList);
        }

        public static void GetSingleShift()
        {
             ResponseDto <ExerciseShiftDto > ShiftsList = Repo.GetAllEntities().GetAwaiter().GetResult();
             int SelectedShiftId = UserInputs<ExerciseShift>.GetShiftById(ShiftsList.Data);
            if(SelectedShiftId==-1)
            {
                return;
            }
            ResponseDto<ExerciseShiftDto> ShiftByIdRsponse=Repo.GetEntiryById(SelectedShiftId).GetAwaiter().GetResult();
             UserOutputs<ExerciseShiftDto>.ShowResponse(ShiftByIdRsponse);
        }

        public static void UpdateShift()
        {
            List<ExerciseShiftDto> ExerciseShifts = Repo.GetAllEntities().GetAwaiter().GetResult().Data;
            int? UserChoice = UserInputs<Exercise>.GetShiftById(ExerciseShifts);
            ResponseDto<ExerciseShiftDto> Response = Repo.GetEntiryById(UserChoice).GetAwaiter().GetResult();
            ExerciseShiftDto UpdatedExerciseShift = UserInputs<ExerciseShiftDto>.GetUpdatedEntity(Response.Data);
            UpdatedExerciseShift = HelperMethods.RefineExerciseShift(UpdatedExerciseShift);
            ResponseDto<ExerciseShiftDto> UpdateResponse = Repo.UpdateEntity(UpdatedExerciseShift, UserChoice).GetAwaiter().GetResult();
            UserOutputs<ExerciseShiftDto>.ShowResponse(UpdateResponse);
        }
    }
}

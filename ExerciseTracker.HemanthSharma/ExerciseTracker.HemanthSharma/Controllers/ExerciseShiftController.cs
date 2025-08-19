using ExerciseTracker.HemanthSharma.HelperMethods;
using ExerciseTracker.Study.Models;
using ExerciseTracker.Study.Models.DTO;
using ExerciseTracker.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTracker.Study.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseShiftController : ControllerBase
{
    private readonly IService<ExerciseShift> ShiftService;
    public ExerciseShiftController(IService<ExerciseShift> service)
    {
        ShiftService = service;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto<ExerciseShift>>> GetAllShifts()
    {
        return await ShiftService.GetAll();
    }

    [HttpGet]
    [Route("{Id:int}")]
    public async Task<ActionResult<ResponseDto<ExerciseShift>>> GetById([FromRoute] int Id)
    {
        return await ShiftService.GetById(Id);
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto<ExerciseShift>>> CreateShift([FromBody] ExerciseShiftDto NewExerciseDto)
    {
        ExerciseShift NewExerciseShift = HelperClass.ConvertToExercise(NewExerciseDto);
        if (NewExerciseShift == null)
        {
            return new ResponseDto<ExerciseShift>
            {
                IsSuccess = false,
                Data = new List<ExerciseShift>(),
                Message = "Bad Data! please check if The Shift Date and Times are in correct format",
                ResponseMethod = "Post"
            };

        }
        return await ShiftService.Create(NewExerciseShift);
    }

    [HttpPut]
    [Route("{Id:int}")]
    public async Task<ActionResult<ResponseDto<ExerciseShift>>> UpdateShift([FromRoute] int Id, [FromBody] ExerciseShiftDto UpdateExerciseDto)
    {
        ExerciseShift UpdateExerciseShift = HelperClass.ConvertToExercise(UpdateExerciseDto);
        if (UpdateExerciseShift == null)
        {
            return new ResponseDto<ExerciseShift>
            {
                IsSuccess = false,
                Data = new List<ExerciseShift>(),
                Message = "Bad Data! please check if The Shift Date and Times are in correct format",
                ResponseMethod = "Post"
            };
        }
        UpdateExerciseShift.Id = Id;
        return await ShiftService.Update(UpdateExerciseShift);
    }

    [HttpDelete]
    [Route("{Id:int}")]
    public async Task<ActionResult<ResponseDto<ExerciseShift>>> DeleteShift([FromRoute] int Id)
    {

        return await ShiftService.Delete(Id);
    }
}

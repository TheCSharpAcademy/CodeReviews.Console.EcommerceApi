using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExerciseTracker.ASV.Db.Data;
using ExerciseTracker.ASV.Db.Models;

namespace ExerciseTracker.ASV.Db.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseDatasController : ControllerBase
{
    private readonly ExerciseDataContext _context;

    public ExerciseDatasController(ExerciseDataContext context)
    {
        _context = context;
    }

    // GET: api/ExerciseDatas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseData>>> GetExercise()
    {
        return await _context.Exercise.ToListAsync();
    }

    // GET: api/ExerciseDatas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseData>> GetExerciseData(int id)
    {
        var exerciseData = await _context.Exercise.FindAsync(id);

        if (exerciseData == null)
        {
            return NotFound();
        }

        return exerciseData;
    }

    // PUT: api/ExerciseDatas/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutExerciseData(int id, ExerciseData exerciseData)
    {
        if (id != exerciseData.Id)
        {
            return BadRequest();
        }

        _context.Entry(exerciseData).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ExerciseDataExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/ExerciseDatas
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ExerciseData>> PostExerciseData(ExerciseData exerciseData)
    {
        _context.Exercise.Add(exerciseData);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetExerciseData", new { id = exerciseData.Id }, exerciseData);
    }

    // DELETE: api/ExerciseDatas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExerciseData(int id)
    {
        var exerciseData = await _context.Exercise.FindAsync(id);
        if (exerciseData == null)
        {
            return NotFound();
        }

        _context.Exercise.Remove(exerciseData);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ExerciseDataExists(int id)
    {
        return _context.Exercise.Any(e => e.Id == id);
    }
}
using Exercisetacker.Data;
using Exercisetacker.DTOs;
using Exercisetacker.Entities;
using Exercisetacker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Exercisetacker.Repositories;

public class JoggingRepository : IJoggingRepository
{
    private readonly JoggingDbContext _context;

    public JoggingRepository(JoggingDbContext context)
    {
        _context = context;
    }

    public async Task<Jogging> AddJogging(Jogging jogging)
    {
        _context.Add(jogging);
        await _context.SaveChangesAsync();
        return jogging;
    }

    public async Task<int> UpdateJogging(Jogging jogging)
    {
        _context.Update(jogging);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteJogging(Jogging jogging)
    {
        _context.Remove(jogging);
        return await _context.SaveChangesAsync();
        
    }

    public async Task<List<Jogging>?> GetAllJogging() => await _context.Joggings.ToListAsync();

    public async Task<Jogging?> GetJoggingById(int id) => await _context.Joggings.FirstOrDefaultAsync(j => j.Id == id);
    
}
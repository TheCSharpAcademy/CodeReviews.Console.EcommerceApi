using Exercisetacker.Data;
using Exercisetacker.DTOs;
using Exercisetacker.Entities;
using Exercisetacker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Exercisetacker.Repositories;

public class JoggingDapperRepository : IJoggingRepository
{
    private readonly JoggingDapperDbContext _context;

    public JoggingDapperRepository(JoggingDapperDbContext context)
    {
        _context = context;
    }

    public async Task<Jogging> AddJogging(Jogging jogging)
    {
        var result = await _context.Add(jogging);
        
        return result;
    }

    public async Task<int> UpdateJogging(Jogging jogging)
    {
        int result = await _context.Update(jogging);
        return result;
    }

    public async Task<int> DeleteJogging(Jogging jogging)
    {
        int result = await _context.Remove(jogging);
        return result;
        
    }

    public async Task<List<Jogging>?> GetAllJogging() => await _context.GetAllAsync();

    public async Task<Jogging?> GetJoggingById(int id) => await _context.FirstOrDefaultAsync(id);
    
}
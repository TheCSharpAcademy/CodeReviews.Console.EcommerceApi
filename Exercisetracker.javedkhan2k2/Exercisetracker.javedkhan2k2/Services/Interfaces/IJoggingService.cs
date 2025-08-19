using Exercisetacker.DTOs;
using Exercisetacker.Entities;

namespace Exercisetacker.Services.Interfaces;

public interface IJoggingService
{
    Task<List<Jogging>?> GetAllJoggings();
    Task<Jogging?> GetJoggingById(int id);
    Task<Jogging> AddJogging(JoggingAddDto jogging);
    Task<int> UpdateJogging(Jogging jogging);
    Task<int> DeleteJogging(Jogging jogging);
}
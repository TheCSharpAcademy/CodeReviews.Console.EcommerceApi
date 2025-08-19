using Exercisetacker.DTOs;
using Exercisetacker.Entities;

namespace Exercisetacker.Repositories.Interfaces;

public interface IJoggingRepository
{
    Task<List<Jogging>?> GetAllJogging();
    Task<Jogging> GetJoggingById(int id);
    Task<Jogging> AddJogging(Jogging jogging);
    Task<int> UpdateJogging(Jogging jogging);
    Task<int> DeleteJogging(Jogging jogging);
}
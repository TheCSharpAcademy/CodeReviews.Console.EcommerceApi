using Exercisetacker.DTOs;
using Exercisetacker.Entities;
using Exercisetacker.Repositories.Interfaces;
using Exercisetacker.Services.Interfaces;

namespace Exercisetacker.Services;

public class JoggingService : IJoggingService
{
    private readonly IJoggingRepository _joggingRepository;

    public JoggingService(IJoggingRepository joggingRepository)
    {
        _joggingRepository = joggingRepository;
    }

    public async Task<Jogging> AddJogging(JoggingAddDto jogging)
    {
        return await _joggingRepository.AddJogging(jogging.ToJogging());
    }

    public async Task<int> DeleteJogging(Jogging jogging) => await _joggingRepository.DeleteJogging(jogging);

    public async Task<List<Jogging>?> GetAllJoggings() => await _joggingRepository.GetAllJogging();

    public async Task<Jogging?> GetJoggingById(int id) => await _joggingRepository.GetJoggingById(id);

    public async Task<int> UpdateJogging(Jogging jogging) => await _joggingRepository.UpdateJogging(jogging);
}
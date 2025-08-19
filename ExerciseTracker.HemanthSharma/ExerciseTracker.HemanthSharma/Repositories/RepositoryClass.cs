using ExerciseTracker.HemanthSharma;
using ExerciseTracker.HemanthSharma.Interfaces;
using ExerciseTracker.Study.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Study.Repositories;

public class RepositoryClass<T> : IRepository<T> where T : class, IEntity<T>
{
    private readonly ContextClass Context;
    private readonly DbSet<T> DbSet;
    public RepositoryClass(ContextClass _context)
    {
        Context = _context;
        DbSet = _context.Set<T>();
    }
    public async Task<ResponseDto<T>> Create(T Entity)
    {
        try
        {
            var EntityResponse = await DbSet.AddAsync(Entity);
            await Context.SaveChangesAsync();
            return new ResponseDto<T>
            {
                IsSuccess = true,
                ResponseMethod = "POST",
                Message = "Successfully Created The Entity",
                Data = new List<T> { EntityResponse.Entity }
            };
        }
        catch (Exception e)
        {
            return new ResponseDto<T>
            {
                IsSuccess = false,
                ResponseMethod = "POST",
                Message = "Error Occured: " + e.Message,
                Data = []
            };
        }
    }

    public async Task<ResponseDto<T>> Delete(int Id)
    {
        try
        {
            var Entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            if (Entity != null)
            {
                DbSet.Remove(Entity);
                Context.SaveChanges();
                return new ResponseDto<T>
                {
                    IsSuccess = true,
                    ResponseMethod = "DELETE",
                    Message = "Successfully Deleted The Entities",
                    Data = [Entity]
                };
            }
            else
            {
                return new ResponseDto<T>
                {
                    IsSuccess = false,
                    ResponseMethod = "DELETE",
                    Message = "No Entity Found to Delete",
                    Data = []
                };
            }
        }
        catch (Exception e)
        {
            return new ResponseDto<T>
            {
                IsSuccess = false,
                ResponseMethod = "DELETE",
                Message = "Error Occured: " + e.Message,
                Data = []
            };
        }
    }

    public async Task<ResponseDto<T>> GetAll()
    {
        try
        {
            var DataList = await DbSet.ToListAsync();
            return new ResponseDto<T>
            {
                IsSuccess = true,
                ResponseMethod = "GET",
                Message = "Successfully Fetched All The Entities",
                Data = DataList
            };
        }
        catch (Exception e)
        {
            return new ResponseDto<T>
            {
                IsSuccess = false,
                ResponseMethod = "GET",
                Message = "Error Occured: " + e.Message,
                Data = []
            };
        }
    }

    public async Task<ResponseDto<T>> GetById(int Id)
    {
        try
        {
            var DataEntity = await DbSet.FindAsync(Id);
            if (DataEntity == null)
            {
                return new ResponseDto<T>
                {
                    IsSuccess = false,
                    ResponseMethod = "GET",
                    Message = "No Entity Found for given ID",
                    Data = new List<T> { DataEntity }
                };
            }
            return new ResponseDto<T>
            {
                IsSuccess = true,
                ResponseMethod = "GET",
                Message = "Successfully Fetched The Entity With Id",
                Data = new List<T> { DataEntity }
            };
        }
        catch (Exception e)
        {
            return new ResponseDto<T>
            {
                IsSuccess = false,
                ResponseMethod = "GET",
                Message = "Error Occured: " + e.Message,
                Data = []
            };
        }
    }

    public async Task<ResponseDto<T>> Update(T Entity)
    {
        try
        {
            if (await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Entity.Id) != null)
            {
                DbSet.Update(Entity);
                Context.SaveChanges();
                return new ResponseDto<T>
                {
                    IsSuccess = true,
                    ResponseMethod = "PUT",
                    Message = "Successfully Updated The Entities",
                    Data = [Entity]
                };
            }
            else
            {
                return new ResponseDto<T>
                {
                    IsSuccess = false,
                    ResponseMethod = "PUT",
                    Message = "No Entity Found to Update",
                    Data = []
                };
            }
        }
        catch (Exception e)
        {
            return new ResponseDto<T>
            {
                IsSuccess = false,
                ResponseMethod = "PUT",
                Message = "Error Occured: " + e.Message,
                Data = []
            };
        }
    }
}

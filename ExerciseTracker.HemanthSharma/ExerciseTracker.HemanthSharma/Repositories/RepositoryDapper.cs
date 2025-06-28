using Dapper;
using ExerciseTracker.Study.Models;
using ExerciseTracker.Study.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Spectre.Console;
using System.Data.Common;

namespace ExerciseTracker.Study.Repositories
{
    public class RepositoryDapper : IRepository<Exercise>
    {
        private string ConnectionString {get;set;}
        private  DbSettings options { get; set; }
        public RepositoryDapper(IOptions<DbSettings> _options)
        {
            options = _options.Value;
            ConnectionString = options.Default;
        }

        public async Task<ResponseDto<Exercise>> Create(Exercise Entity)
        {
            AnsiConsole.MarkupLine($"[Yellow]Creating The Exercise[darkblue] {Entity.Name}[/] Through[/] [orange3]Dapper[/]");
            try
            {
                using (SqlConnection conn = new(ConnectionString))
                {
                    string CreateQuery = "Insert Into Exercises([Name]) Values(@Name);";
                    int RowsAffected=conn.Execute(CreateQuery, new { Name = Entity.Name });
                    AnsiConsole.MarkupLine($"The Following Rows are Affected {RowsAffected}");
                    return new ResponseDto<Exercise>
                    {
                        IsSuccess = true,
                        ResponseMethod = "POST",
                        Data = [Entity],
                        Message = "Successfully Created Entity using Dapper"
                    };
                }
            }
            catch(Exception e)
            {
                AnsiConsole.MarkupLine($"Something Wrong {e.Message}");
                return new ResponseDto<Exercise>
                {
                    IsSuccess = false,
                    ResponseMethod = "POST",
                    Data = [],
                    Message = e.Message
                };
            }
            

        }

        public async Task<ResponseDto<Exercise>> Delete(int Id)
        {
            AnsiConsole.MarkupLine($"[Yellow]Deleting The Exercise[darkblue] [/] Through[/] [orange3]Dapper[/]");
            try
            {
                using (SqlConnection conn = new(ConnectionString))
                {
                    string DeleteQuery = "Delete from Exercises where Id=@id ;";
                    int RowsAffected = conn.Execute(DeleteQuery, new { id = Id });
                    AnsiConsole.MarkupLine($"The Following Rows are Affected {RowsAffected}");
                    return new ResponseDto<Exercise>
                    {
                        IsSuccess = true,
                        ResponseMethod = "DELETE",
                        Data = [],
                        Message = "Successfully Deleted Entity using Dapper"
                    };
                }
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"Something Wrong {e.Message}");
                return new ResponseDto<Exercise>
                {
                    IsSuccess = false,
                    ResponseMethod = "DELETE",
                    Data = [],
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseDto<Exercise>> GetAll()
        {
            try
            {
                using (SqlConnection conn = new(ConnectionString))
                {
                    string GetAllQuery = "Select * from Exercises ;";
                    List<Exercise> Exercises= conn.Query<Exercise>(GetAllQuery).ToList();
                    return new ResponseDto<Exercise>
                    {
                        IsSuccess = true,
                        ResponseMethod = "GET",
                        Data = Exercises,
                        Message = "Successfully Retrieved All the Entities using Dapper"
                    };
                }
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"Something Wrong {e.Message}");
                return new ResponseDto<Exercise>
                {
                    IsSuccess = false,
                    ResponseMethod = "GET",
                    Data = [],
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseDto<Exercise>> GetById(int Id)
        {
            try
            {
                using (SqlConnection conn = new(ConnectionString))
                {
                    string GetQueryById = "Select * from Exercises where Id=@id ;";
                    List<Exercise> Exercises = conn.Query<Exercise>(GetQueryById, new {id=Id}).ToList();
                    return new ResponseDto<Exercise>
                    {
                        IsSuccess = true,
                        ResponseMethod = "GET",
                        Data = Exercises,
                        Message = "Successfully Retrieved All the Entities using Dapper"
                    };
                }
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"Something Wrong {e.Message}");
                return new ResponseDto<Exercise>
                {
                    IsSuccess = false,
                    ResponseMethod = "GET",
                    Data = [],
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseDto<Exercise>> Update(Exercise Entity)
        {
            try
            {
                using (SqlConnection conn = new(ConnectionString))
                {
                    string UpdateQuery = "Update Exercises" +
                                          " set Name=@name where Id=@id;";
                    int RowsAffected = conn.Execute(UpdateQuery, new { Name = Entity.Name, id=Entity.Id});
                    AnsiConsole.MarkupLine($"The Following Rows are Affected {RowsAffected}");
                    return new ResponseDto<Exercise>
                    {
                        IsSuccess = true,
                        ResponseMethod = "PUT",
                        Data = [Entity],
                        Message = "Successfully Updated Entity using Dapper"
                    };
                }
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"Something Wrong {e.Message}");
                return new ResponseDto<Exercise>
                {
                    IsSuccess = false,
                    ResponseMethod = "PUT",
                    Data = [],
                    Message = e.Message
                };
            }
        }
    }
}

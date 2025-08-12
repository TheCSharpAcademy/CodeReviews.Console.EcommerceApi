using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ExerciseTrackerAPI;

public class ApiClient
{
    private static readonly HttpClient httpClient = new HttpClient()
    {
        BaseAddress = new Uri("https://localhost:7136")
    };

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        var response = await httpClient.GetAsync("/api/Customer");
        if (!response.IsSuccessStatusCode)
            return null;

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<Customer>>();

        return result;
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        var response = await httpClient.GetAsync($"/api/Customer/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Customer>();

        return result;
    }

    public async Task<Customer> AddCustomerAsync(string firstName, string lastName, string phoneNumber, string password)
    {
        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Password = password
        };

        try
        {
            var json = JsonSerializer.Serialize(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/Customer", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Customer>>(responseJson);

            return result?.LastOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding customer:");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<Customer> UpdateCustomerAsync(int id, string firstName, string lastName, string phoneNumber, string password)
    {
        var customer = new Customer { Id = id, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, Password = password };

        try
        {
            var json = JsonSerializer.Serialize(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"/api/Customer/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var updatedCustomer = JsonSerializer.Deserialize<Customer>(responseJson);

            return updatedCustomer;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating customer:");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/Customer/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Exercise>> GetAllExercisesAsync()
    {
        var response = await httpClient.GetAsync("/api/Exercise");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<Exercise>>();

        return result;
    }

    public async Task<Exercise> GetExerciseByIdAsync(int id)
    {
        var response = await httpClient.GetAsync($"/api/Exercise/{id}");
        if (!response.IsSuccessStatusCode)
            return null;
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Exercise>();

        return result;
    }

    public async Task<List<Exercise>> GetExercisesByCustomerIdAsync(int customerId)
    {
        var exercises = await GetAllExercisesAsync();
        var filteredExercises = exercises.Where(e => e.CustomerId == customerId).ToList();

        return filteredExercises;
    }

    public async Task<Exercise> AddExerciseAsync(string Name, DateTime dateStart, DateTime dateEnd, int repetitions, int customerId, string comments)
    {
        var exercise = new Exercise
        {
            Name = Name,
            DateStart = dateStart,
            DateEnd = dateEnd,
            Repetitions = repetitions,
            CustomerId = customerId,
            Comments = comments
        };

        try
        {
            var json = JsonSerializer.Serialize(exercise);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/Exercise", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Exercise>>(responseJson);

            return result?.LastOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding exercise:");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<Exercise> UpdateExerciseAsync(int id, DateTime dateStart, DateTime dateEnd, int repetitions, int customerId, string comments)
    {
        var exercise = new Exercise
        {
            Id = id,
            DateStart = dateStart,
            DateEnd = dateEnd,
            Repetitions = repetitions,
            CustomerId = customerId,
            Comments = comments
        };

        try
        {
            var json = JsonSerializer.Serialize(exercise);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"/api/Exercise/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var updatedExercise = JsonSerializer.Deserialize<Exercise>(responseJson);

            return updatedExercise;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating exercise:");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool?> DeleteExerciseAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/Exercise/{id}");
        if (!response.IsSuccessStatusCode)
            return null;
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteExercisesByCustomerIdAsync(int customerId)
    {
        var exercises = await GetAllExercisesAsync();

        bool success = true;

        foreach (var exercise in exercises)
        {
            if (exercise.CustomerId == customerId)
            {
                bool? result = await DeleteExerciseAsync(exercise.Id);
                if (result == null)
                {
                    success = false;
                    break;
                }
            }
        }

        return success;
    }

    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }

    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan Duration => DateEnd - DateStart;
        public int Repetitions { get; set; }
        public int CustomerId { get; set; }
        public string Comments { get; set; } = string.Empty;
    }
}

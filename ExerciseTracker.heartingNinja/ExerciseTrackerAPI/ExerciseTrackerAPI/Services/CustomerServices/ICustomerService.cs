namespace ExerciseTrackerAPI.Services.CustomerServices;

public interface ICustomerService
{
    Task<List<Customer>> GetAll();
    Task<Customer> Get(int id);
    Task<List<Customer>> AddCustomer(Customer customer);
    Task<Customer>? UpdateCustomer(int id, Customer request);
    Task<List<Customer>> Delete(int id);
}

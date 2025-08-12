namespace ExerciseTrackerAPI.Services.CustomerServices;

public class CustomerService : ICustomerService
{
    private readonly DataContext _context;

    public CustomerService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return await _context.Customers.ToListAsync();
    }

    public async Task<List<Customer>> Delete(int id)
    {
        var dbCustomer = await _context.Customers.FindAsync(id);
        if (dbCustomer == null)
            return new List<Customer>();

        _context.Customers.Remove(dbCustomer);
        await _context.SaveChangesAsync();

        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> Get(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return null;

        return customer;
    }

    public async Task<List<Customer>> GetAll()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> UpdateCustomer(int id, Customer request)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return null;

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.PhoneNumber = request.PhoneNumber;
        customer.Password = request.Password;

        await _context.SaveChangesAsync();

        return customer;
    }
}

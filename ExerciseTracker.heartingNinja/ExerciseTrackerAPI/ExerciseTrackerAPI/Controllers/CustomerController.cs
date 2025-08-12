using ExerciseTrackerAPI.Services.CustomerServices;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customer;

    public CustomerController(ICustomerService customer)
    {
        _customer = customer;
    }

    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        return await _customer.GetAll();
    }

    [HttpPost]
    public async Task<ActionResult<List<Customer>>> AddCustomer(Customer customer)
    {
        var results = await _customer.AddCustomer(customer);
        return results;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> Get(int id)
    {
        var results = await _customer.Get(id);
        if (results is null)
            return NotFound();

        return Ok(results);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<List<Customer>>> UpdateCustomer(int id, Customer response)
    {
        var results = await _customer.UpdateCustomer(id, response);
        if (results is null)
            return NotFound();

        return Ok(results);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<Customer>>> Delete(int id)
    {
        var results = await _customer.Delete(id);
        if (results is null)
            return NotFound();

        return Ok(results);
    }
}

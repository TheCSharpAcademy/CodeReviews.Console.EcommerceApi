using ConsoleTableExt;
using ExerciseTrackerAPI;
using static ExerciseTrackerAPI.ApiClient;

namespace ExerciseTrackerConsoleApp;

internal class CustomerInfoInput
{

    static bool deleteCustomer;
    static bool updateCustomer;
    static bool getOneCustomer;
    internal static async Task CustomerUI()
    {
        var apiClient = new ApiClient();

        Console.Clear();
        Console.WriteLine("Customer Options:");
        Console.WriteLine("1. List all customers");
        Console.WriteLine("2. Get a customer by ID");
        Console.WriteLine("3. Add a customer");
        Console.WriteLine("4. Update an existing customer");
        Console.WriteLine("5. Delete an existing customer");
        Console.WriteLine("6. Go Back");

        while (true)
        {
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await ListCustomersAsync(apiClient);
                    break;

                case "2":
                    getOneCustomer = true;
                    await ListCustomersAsync(apiClient);
                    await GetCustomerAsync(apiClient);
                    break;

                case "3":
                    await AddCustomerAsync(apiClient);
                    break;

                case "4":
                    updateCustomer = true;
                    await ListCustomersAsync(apiClient);
                    await UpdateCustomerAsync(apiClient);
                    break;

                case "5":
                    deleteCustomer = true;
                    await ListCustomersAsync(apiClient);
                    await DeleteCustomerAsync(apiClient);
                    break;

                case "6":
                    await MainMenu.StartUI();
                    break;

                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    internal static async Task ListCustomersAsync(ApiClient apiClient)
    {
        Console.WriteLine("Listing all customers...");

        var customers = await apiClient.GetAllCustomersAsync();

        ConsoleTableBuilder
            .From(customers)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);

        if (!deleteCustomer && !updateCustomer && !getOneCustomer && !ExerciseInput.getCustomers)
        {
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await CustomerUI();
        }
    }

    private static async Task GetCustomerAsync(ApiClient apiClient)
    {
        getOneCustomer = false;
        Console.WriteLine("Enter the ID of the customer (or 'b' to go back to Menu):");
        string input = Console.ReadLine();

        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await CustomerUI();
        }

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid ID, please try again.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await GetCustomerAsync(apiClient);
        }

        var customer = await apiClient.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await GetCustomerAsync(apiClient);
        }

        Console.WriteLine($"Getting customer with ID {customer.Id}...");
        var tableData = new List<Customer> { customer };

        ConsoleTableBuilder
        .From(tableData)
        .WithFormat(ConsoleTableBuilderFormat.Alternative)
        .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await CustomerUI();
    }

    internal static async Task AddCustomerAsync(ApiClient apiClient)
    {
        Console.WriteLine("Enter the first name of the new customer:");
        var firstName = Console.ReadLine();

        Console.WriteLine("Enter the last name of the new customer:");
        var lastName = Console.ReadLine();

        Console.WriteLine("Enter the phone number of the new customer:");
        var phoneNumber = Console.ReadLine();

        Console.WriteLine("Enter the password of the new customer:");
        var password = Console.ReadLine();

        Console.WriteLine($"Adding new customer {firstName}...");

        await apiClient.AddCustomerAsync(firstName, lastName, phoneNumber, password);
        await PrintLastCustomerAsync(apiClient);

        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await CustomerUI();
    }

    internal static async Task PrintLastCustomerAsync(ApiClient apiClient)
    {
        var customers = await apiClient.GetAllCustomersAsync();

        if (customers.Count > 0)
        {
            var lastCustomer = customers[^1];
            var tableData = new List<Customer> { lastCustomer };

            ConsoleTableBuilder
                .From(tableData)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Center);
        }
        else
        {
            Console.WriteLine("No customers found.");
        }
    }

    private static async Task UpdateCustomerAsync(ApiClient apiClient)
    {
        updateCustomer = false;
        Console.WriteLine("Enter the ID of the customer (or 'b' to go back to Menu):");
        string input = Console.ReadLine();

        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await CustomerUI();
        }

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid ID, please try again.");
            await UpdateCustomerAsync(apiClient);
        }

        var customer = await apiClient.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await UpdateCustomerAsync(apiClient);
        }

        Console.WriteLine("Enter the new name of the customer or Enter to keep the same:");
        var firstName = Console.ReadLine();

        if (string.IsNullOrEmpty(firstName))
        {
            firstName = customer.FirstName;
        }

        Console.WriteLine("Enter the new last name of the customer or Enter to keep the same:");
        var lastName = Console.ReadLine();

        if (string.IsNullOrEmpty(lastName))
        {
            lastName = customer.LastName;
        }

        Console.WriteLine("Enter the new phone number of the customer or Enter to keep the same:");
        var phoneNumber = Console.ReadLine();

        if (string.IsNullOrEmpty(phoneNumber))
        {
            phoneNumber = customer.PhoneNumber;
        }

        Console.WriteLine("Enter the new password of the customer or Enter to keep the same:");
        var password = Console.ReadLine();

        if (string.IsNullOrEmpty(password))
        {
            password = customer.Password;
        }

        await apiClient.UpdateCustomerAsync(id, firstName, lastName, phoneNumber, password);
        Console.WriteLine($"Customer with name: {firstName} updated.");
        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await CustomerUI();
    }

    private static async Task DeleteCustomerAsync(ApiClient apiClient)
    {
        deleteCustomer = false;
        Console.WriteLine("Enter the ID of the customer to delete (or 'b' to go back to Menu):");
        string input = Console.ReadLine();

        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await CustomerUI();
        }

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid ID, please try again.");
            await DeleteCustomerAsync(apiClient);
        }

        var customer = await apiClient.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await DeleteCustomerAsync(apiClient);
        }

        bool success = await apiClient.DeleteExercisesByCustomerIdAsync(id);

        if (success)
        {
            Console.WriteLine($"Exercises with customer ID: {id} deleted.");
        }
        else
        {
            Console.WriteLine($"No exercises found for customer with ID: {id}.");
        }

        Console.WriteLine($"Deleting customer with ID {customer.Id}...");
        await apiClient.DeleteCustomerAsync(id);
        Console.WriteLine($"Customer with ID {customer.Id} deleted.");
        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await CustomerUI();
    }
}

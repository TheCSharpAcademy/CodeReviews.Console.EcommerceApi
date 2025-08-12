using ExerciseTrackerAPI;

namespace ExerciseTrackerConsoleApp;

internal class UserInput
{
    internal static async Task UserUI()
    {
        var apiClient = new ApiClient();

        Console.Clear();
        Console.WriteLine("User Options:");
        Console.WriteLine("1. Sign In");
        Console.WriteLine("2. Create New Account");
        Console.WriteLine("3. Go Back");

        while (true)
        {
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await SignIn(apiClient);
                    break;

                case "2":
                    await AddNewCustomer(apiClient);
                    break;

                case "3":
                    await MainMenu.StartUI();
                    break;

                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    private async static Task AddNewCustomer(ApiClient apiClient)
    {
        Console.WriteLine("Enter the first name of the new customer: (or 'b' to go back to Menu):");
        var firstName = Console.ReadLine();

        if (firstName.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await UserUI();
        }

        Console.WriteLine("Enter the last name of the new customer:");
        var lastName = Console.ReadLine();

        Console.WriteLine("Enter the phone number of the new customer:");
        var phoneNumber = Console.ReadLine();

        Console.WriteLine("Enter the password of the new customer:");
        var password = Console.ReadLine();

        Console.WriteLine($"Adding new customer {firstName}...");
        await apiClient.AddCustomerAsync(firstName, lastName, phoneNumber, password);
        await CustomerInfoInput.PrintLastCustomerAsync(apiClient);
        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await UserUI();
    }

    private static async Task SignIn(ApiClient apiClient)
    {
        Console.WriteLine("Enter your customer ID: (or 'b' to go back to Menu):");
        string customerIdInput = Console.ReadLine();

        if (customerIdInput.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await UserUI();
        }

        if (!int.TryParse(customerIdInput, out int customerId))
        {
            Console.WriteLine("Invalid customer ID format. Please try again.");
            await SignIn(apiClient);
        }

        var customer = await apiClient.GetCustomerByIdAsync(customerId);

        bool isValid = false;
        Console.WriteLine("Enter your password:");
        string password = Console.ReadLine();

        if (customer != null && password == customer.Password)
        {
            isValid = true;
            Console.WriteLine($"{customer.FirstName} signed in");
        }

        if (!isValid)
        {
            Console.WriteLine("Invalid customer ID or password.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await UserUI();
        }

        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await UserSignedInInput.UserMenu(customerId);
    }
}

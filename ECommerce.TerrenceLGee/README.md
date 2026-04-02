# E-Commerce Application
An e-commerce application with a back end web API and a front end GUI client. Created with ASP.NET Core and Avalonia.

Created following the curriculum of [C# Academy](https://www.thecsharpacademy.com/)

[E-Commerce](https://www.thecsharpacademy.com/project/18/ecommerce-api)

## Features
- Has both admin and customer functionality each with it's own menu and abilities.
- Implements password authentication and role authorization using ASP.NET Core Identity.
- Uses JWT tokens for authorization.
- Allows the admin user to add and update categories, add, update, mark as deleted and restore products in a category as well as update the status of a customer's order.
- Allows the customer to shop, cancel an order, retrieve individual orders, and add billing/shipping addresses.
- Seeds the database with both a admin user and a customer (with orders) as well as seeds categories and products.
- Uses the Result pattern from the service layer.
- Uses a custom API Response from the controllers.
- Use pagination/sorting/filtering on the back end and pagination and filtering on the front end (sorting can be done from the data grid on the front end).
- Implements logging to file in both the API and the GUI client.
- Comes with a postman collection for testing the backend functionalities.

## Explanation Of Certain Choices Made For This Project
- Avalonia - In the project requirements there was actually not requirement for any kind of front end to consume the API and as a challenge it was only asked to create a console application to consume the API. But in order to stretch my self and my knowledge I decided to attempt a desktop/GUI application as I have also been interested in that in the past.
- I decided to use Avalonia because it is cross-platform and I am not certain of what operating system the code reviewer could be using.
- The reason I choose the have both an admin user and customers is because the roles of each type of user would be different with different responsibilities and again in the interest of improving my development skills I really tried to implement many different things.
- You will also notice that I have the JWT key in the appsettings.json configuration file. That is obviously not secure, but because this was not part of the project requirements I felt it best to leave the key in the configuration file instead of forcing the reviewer/end-user to attempt to generate their own JWT key. 
- I decided to put the DTOs in a shared library that both the API and the Avalonia application had a reference to in order to prevent code duplication.
- Normally I would have also done unit tests with this project but because this is a learning project that is part of the C# academy's road map and it took me so long to completed because I decided to implement a desktop application, I decided to forgo them at this time, but later on I would like to return to this project and implement unit testing.

## Challenges Face When Implementing This Project
- Not so much a challenge more wrapping my mind around and understanding many-to-many database table relationships.
- Implementing the ordering/creating a sale functionality. Again not so much a challenge more so just wrapping my mind around certain concepts and how to structure the "algorithm" to calculate a sale.
- The biggest challenge was obviously the one that was not even part of the project requirements and that was the Avalonia application part of the project. That part alone took about a good month (it took about a month and half to two months for this entire project from the time I started researching how to implement certain things to completion). Wrapping my mind around the concept of View Models and View as different from what I have been used to doing in previous projects but I learned so much and it is so different from the way I would have done with a console app. For example in a console app for the admin add product functionality I would have probably created separate methods to view the categories available to choose to add a product to and then had a method to take the information from the admin user (e.g. product name, description etc.) and then basically displayed on the screen a success or failure message. But basically for each part that I just described thee was a separate View Model and corresponding View for it. That meant I had to not only create the C# code for the View Model and the functionalities being implemented but also I had to get up to speed with "axaml" so that was definitely a challenge at first.

## What I Learned Implementing This Project
- Learned even more than I did on the last project about implementing a API with ASP.NET Core because the last project basically had one main functionality (logging shifts) while this one has many and there needed to be endpoints for everything that needed to be done from making a sale to adding a category.
- Learned how to use a handler class to deal with passing the authentication bearer token to the endpoints.
- Learned the importance of creating code libraries for reusable code across different projects.
- Learned not only how complex putting together an application can be when there are different parts/projects that have to work together but just how fun and exciting and most of all satisfying it is when something works like you want it to and also how satisfying it is when something doesn't work and you get to "investigate" with the debugger and also reviewing the code you wrote and finding mistakes and correcting them.
- Learned a lot about the MVVM pattern as well as topics such as subscribing to events and messaging to communicate information between classes.


# Instructions
In order to create and seed the database you must go into the appsettings.json file in the E-Commerce.Api project and enter the SQL Server database connection string for example like the following:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ECommerce-Db;Trusted_Connection=True;"
  }
```

In order to use the program properly you will have run both the API and the Avalonia application simultaneously. The most basic way is from the command line interface. This is also necessary if you are using Visual Studio Code.
It's best to open two terminal windows: One in the API project root directory and other in the Avalonia project root directory. For the API root directory run the following commands:
```
dotnet build && dotnet run --launch-profile https
```
Then from the Avalonia project root directory
```
dotnet build && dotnet run
```

For Visual Studio: The following link will take you to instructions on starting multiple projects in Visual Studio:

[Start multiple projects in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=visualstudio)

For Jetbrains Rider: The following link will take you to instructions on starting multiple projects in Jetbrains Rider:

[Start multiple projects in Jetbrains Rider](https://www.jetbrains.com/help/rider/Run_Debug_Multiple.html)

# Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later must be installed
- SQL Server or SQL Server LocalDB must be installed
- An active internet connection is required when first running the Avalonia client 
  as product images are loaded from the web

## IDE Specific Requirements

### Visual Studio
- No additional extensions are required for Avalonia as the project runs via the 
  .NET CLI, however if you would like design time previewing of Avalonia views you 
  can optionally install the Avalonia for Visual Studio extension:
  [Avalonia for Visual Studio](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaVS)

### JetBrains Rider
- Rider has built in support for Avalonia with no additional plugins required
- However for the best Avalonia design time experience you can optionally install 
  the Avalonia plugin:
  [Avalonia for Rider](https://plugins.jetbrains.com/plugin/14839-avaloniaui)

### Visual Studio Code
- The following extension is recommended for Avalonia syntax highlighting and 
  previewing:
  [Avalonia for VSCode](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.vscode-avalonia)

# First Time Setup
Before running the application for the first time you may need to trust the 
.NET developer SSL certificate. Run the following command once:
```
dotnet dev-certs https --trust
```
This prevents browser and HTTP client SSL errors when the Avalonia client 
communicates with the API over HTTPS.

# Troubleshooting

### The Avalonia application cannot connect to the API
- Ensure the API is running before launching the Avalonia client
- Check that the API is running on the expected port — you can verify this in 
  the API project's launchSettings.json file under the https profile
- Ensure the BaseUrl in the Avalonia client's Urls.cs or equivalent configuration 
  file matches the port the API is running on

### Database errors on startup
- Ensure your connection string in appsettings.json is correct
- The application uses Entity Framework Core migrations to create and seed the 
  database automatically on first run — ensure your SQL Server instance is running
- If you encounter migration errors try running the following from the API project 
  root directory:
```
dotnet ef database update
```

### Windows only — Mica transparency effect
- The Avalonia client uses the Windows 11 Mica transparency effect for its window 
  background. This is a cosmetic feature only and will gracefully fall back to a 
  solid dark background on Windows 10 or Linux/macOS with no impact on functionality.

### Linux or macOS
- The Avalonia client should run on Linux and macOS via the .NET CLI as Avalonia 
  is cross platform, however the application has been developed and tested primarily 
  on Windows. Minor visual differences may be present on other operating systems.

## When the program is up and running
- Once the Avalonia application starts (after the API has finished loading) you can either create a register a new user you can use the either the default user/customer or the default admin user the tasks that you are able to perform will depend upon which role you log in as (admin or customer). A corresponding menu will be displayed based on the role of the logged in user

The default admin user is:
```
username: admin@example.com
password: Pa$$w0rd
```

The default user is:
```
username: tjones@example.com
password: Pa$$w0rd
```
## Areas To Improve Upon
- Becoming more knowledgeable about using the C# language and taking advantage of all it has to offer.
- I need to study design patterns because I honestly don't know if I designed this application in the most efficient way.

## Technologies Used
- [.NET10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Avalonia](https://avaloniaui.net/)
- [MessageBox.Avalonia](https://github.com/AvaloniaCommunity/MessageBox.Avalonia)
- [Serilog](https://serilog.net/)
- [Microsoft.EntityFrameworkCore](https://learn.microsoft.com/en-us/ef/)
- [Microsoft SQL Server](https://learn.microsoft.com/en-us/sql/?view=sql-server-ver17)
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio)

## A Few Helpful Resources Used 
- [DotNetTutorials.net Web API Tutorials](https://dotnettutorials.net/course/asp-net-core-web-api-tutorials/)
- [How to Customize ASP.NET Core Identity With EF Core](https://antondevtips.com/blog/how-to-customize-aspnet-core-identity-with-efcore-for-your-project-needs)
- [CommunityToolkit.Mvvm documentation](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [Avalonia Documentation](https://docs.avaloniaui.net/docs/welcome)
- [Awaiting an async lambda passed as a parameter](https://www.pixata.co.uk/2025/04/21/awaiting-an-async-lambda-passed-as-a-parameter/)
- [StackOverflow](https://stackoverflow.com/)

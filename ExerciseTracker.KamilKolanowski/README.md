# Exercise Tracker Console App

A simple C# console application to help users track their training and exercise sessions. The app supports two types of exercises: **Weight** and **Cardio**.

## Features

- Track and log your exercise sessions
- Two exercise types:
  - **Weight**: Traditional weight-lifting sessions, managed via Entity Framework Core and the Repository Pattern
  - **Cardio**: Cardio workouts, managed using Dapper and pure SQL scripts as a coding challenge
- Data persistence using a local SQL Server database

## Technologies Used

- C#
- .NET Core Console Application
- Entity Framework Core (for Weight exercises)
- Dapper (for Cardio exercises)
- Repository Pattern

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed
- Access to a SQL Server instance (tested with `localhost`)

### Setup Instructions

1. **Clone the repository**
    ```bash
    git clone https://github.com/KamilKolanowski/CodeReviews.Console.ExerciseTracker.git
    cd ExerciseTracker.KamilKolanowski
    ```

2. **Restore NuGet packages**
    ```bash
    dotnet restore
    ```

3. **Configure your database connection**

   Edit the `appsettings.json` file and update the connection string with your SQL Server credentials (pointing to localhost):

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ExerciseTrackerDb;Trusted_Connection=True;"
     }
   }
   ```

4. **Create the database**

    ```bash
    dotnet ef database update
    ```

    This command sets up the database and applies all necessary migrations.

5. **Run the application**

    ```bash
    dotnet run
    ```

## Usage

- Choose between **Weight** or **Cardio** exercise tracking.
- Log your sessions and view your history.
- Weight exercises are handled with EF Core; Cardio exercises are handled with Dapper and raw SQL.

## Notes

- The application uses different data access approaches for each exercise type as a coding challenge.
- Make sure your SQL Server is running and accessible on `localhost` or update the connection string accordingly.

## License

This project is provided for educational purposes.
# ExerciseTracker.Study

## Overview

**ExerciseTracker.Study** is a C# project designed to help users track and manage their exercise routines efficiently. This application provides tools to log workouts, monitor progress, and analyze fitness trends, making it ideal for individuals and groups aiming to improve their physical health.

## Features

- Log daily exercises with details such as type, duration, and intensity
- View and analyze workout history and trends
- Set and track fitness goals
- User-friendly interface
- Data persistence for workout records

## Getting Started
## Build Instructions
### DataBaseConnectionString
Change the Connection string in appsettings.json in ExerciseTracker API project to map to your local Db and run migration.
### BaseUrl in in Console project
Chnage the BaseUrl in ExerciseTracker.UI project in app.Config file. to point to the localhost port where your webAPI project runs
### Running Both the Projects as Startup Projects
right click on any of project-> Configure StartUp projects-> select Multiple Startup projects-> select both the projects listed as Start projects

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version X.X or above)
- A compatible development environment (e.g., Visual Studio, VS Code)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/HKHemanthsharma/CodeReviews.Console.ExerciseTracker.git
   cd ExerciseTracker.Study
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the project:**
   ```bash
   dotnet build
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

## Usage

- Launch the application and follow the on-screen instructions to start logging workouts.
- Navigate through features to view progress reports and set new goals.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any feature requests, bug reports, or improvements.

## License

This project is licensed under the [MIT License](LICENSE).

## Maintainers

- [HKHemanthsharma](https://github.com/HKHemanthsharma)

---

*Happy exercising!*

# ExerciseTracker

This is a C# application for The C# Academy roadmap. It utilizes the repository pattern with dependency injection to run a service that performs CRUD operations against a SQLite database.

## Project Requirements

To meet the requirements specified by The C# Academy, the project must do the following:
- This is an application where you should record exercise data.
- You should choose one type of exercise only. We want to keep the app simple so you focus on the subject you're learning and not on the business complexities.
- You can choose raw SQL or Entity Framework for your data-persistence.
- The model for your exercise class should have at least the following properties: `{Id INT, DateStart DateTime, DateEnd DateTime, Duration TimeSpan, Comments string}`
- Your application should have the following classes: `UserInput`, `ExerciseController`, `ExerciseService` (where business logic will be handled) and `ExerciseRepository`. These classes might feel empty at first but they'll be needed in most applications as they grow.
- You can choose between SQLite or SQLServer.
- You need to use dependency injection to access the repository from the controller.

## Notes

I originally used ADO.Net in the `ExerciseRepository` to perform database operations. Once I had that working I added `ExerciseRepositoryEntity` that uses Entity Framework to perform database operations.
The point was to illustrate the separation of concerns the repository pattern provides. You can test this out by going into the `ExerciseService` constructor and changing `ExerciseRepository` to `ExerciseRepositoryEntity`.

![image](https://github.com/user-attachments/assets/fb153440-f1ba-45c6-ba34-5d6a248357f2)

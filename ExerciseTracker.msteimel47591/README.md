# ExerciseTracker

This is a C# application for The C# Academy roadmap. It utilizes the repository pattern with dependency injection to run a service that performs CRUD operations against a SQLite database.

## Project Requirements

To meet the requirements specified by The C# Academy, the project must do the following:

- Record exercise data.
- Choose one type of exercise only to keep the app simple and focused on the subject you're learning.
- Use either raw SQL or Entity Framework for data persistence.
- The model for your exercise class should have at least the following properties:
  - `Id` (INT)
  - `DateStart` (DateTime)
  - `DateEnd` (DateTime)
  - `Duration` (TimeSpan)
  - `Comments` (string)
- Include the following classes:
  - `UserInput`
  - `ExerciseController`
  - `ExerciseService` (handles business logic)
  - `ExerciseRepository`
- Choose between SQLite or SQLServer.
- Use dependency injection to access the repository from the controller.

## Notes

I originally used ADO.Net in the `ExerciseRepository` to perform database operations. Once I had that working, I added `ExerciseRepositoryEntity` that uses Entity Framework to perform database operations. The point was to illustrate the separation of concerns the repository pattern provides. You can test this out by going into the `ExerciseService` constructor and changing `ExerciseRepository` to `ExerciseRepositoryEntity`.

![image](https://github.com/user-attachments/assets/fb153440-f1ba-45c6-ba34-5d6a248357f2)
# Exercise Tracker application
This is an exercise tracker application developed using C# Entity Framework & SQL Server. The app consists of a console UI offering CRUD methods for tracking exercises.

## Requirements / Description
1) This is an application where you should record exercise data.
2) You should choose one type of exercise only. We want to keep the app simple so you focus on the subject you're learning and not on the business complexities.
3) The model for your exercise class should have at least the following properties: {Id INT, DateStart DateTime, DateEnd DateTime, Duration TimeSpan, Comments string}
4) Your application should have the following classes: UserInput, ExerciseController, ExerciseService (where business logic will be handled) and ExerciseRepository.
5) You need to use dependency injection to access the repository from the controller.

## Before using the application
* After cloning the application, update the *connectionStrings* in App.config with your connection string to target your SQL Server.
* Data Source=*YourServerName* Initial Catalog=*YourDatabaseName*.
* Start the application.

## General Info
* The console application consists of menu presenting CRUD options for Tracking Exercises.
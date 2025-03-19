# Exercise Tracker Application

## Features 
* This is an application where we record exercise data.

* The model for the exercise class has the following properties: {Id INT, DateStart DateTime, DateEnd DateTime, Duration TimeSpan, Description string}

* Entity Framework is used for data-persistence.

* The application has the following classes: UserInput, ExerciseController, ExerciseService (where business logic is handled) and ExerciseRepository.

* Microsoft SQLServer as the database.

* The controller of the application is made lean by using dependency injection to access the Exercise Service. The Exercise Service also uses DI to access the Exercise Repository.


## To run create a .env file in the root directory of the Database project(ExerciseTracker.ASV.Db) and add the following properties
* CONNECTION_STRING=Connection string to your database

To create database locally run the below command in your NuGet Package Manager Console:
* dotnet ef migrations add InitialCreate
* dotnet ef database update

Now start the  Web API(ExerciseTracker.ASV.DB) project and the console app(ExerciseTracker.ASV) separately.
   

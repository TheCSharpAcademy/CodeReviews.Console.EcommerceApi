# Excersie Tracking Console Application

## Project Description

This Application is Part of Console Application Project
at [CSharpAcademy](https://thecsharpacademy.com/project/15/drinks).

## The Application Requirements

* This is an application where you should record exercise data.
* You should choose one type of exercise only. We want to keep
the app simple so you focus on the subject you're learning and
not on the business complexities.
* You can choose raw SQL or Entity Framework for your data-persistence.
The model for your exercise class should have at least the following
properties: {Id INT, DateStart DateTime, DateEnd DateTime, Duration
TimeSpan, Comments string}
* Your application should have the following classes: UserInput,
ExerciseController, ExerciseService (where business logic will
be handled) and ExerciseRepository. These classes might feel empty
at first but they'll be needed in most applications as they grow.
* You can choose between SQLite or SQLServer.
* You need to use dependency injection to access the repository
from the controller.

## How to run the Application

Microsoft [Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows)
is used to store and retrieve Database Connection. To run the
application set the ConnectionStrings:DefaultConnection in your Project.  
e.g. dotnet user-secrets ConnectionStrings:DefaultConnection
= Server=localhost;Database=Exercisetracker;
User Id=YourUserID;Password=YourPassword;TrustServerCertificate=True;  
First run the Shiftlogger.javedkhan2k2 Project. This act as the API.  
Now run the Shiftlogger.UI to interact with the APIs.

## Application Usage

Users can add/delete/edit/update Jogging Session from the Main menu.

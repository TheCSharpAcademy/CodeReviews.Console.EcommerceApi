# Exercise Tracker

This is a simple exercise tracker application built for a console-based user interface. The application allows users to record exercise data, including the start and end time, duration, and comments.

## Project Overview

The goal of this project is to learn and implement the **Repository Pattern** for data persistence. This pattern helps create a layer between business logic and data access, promoting loosely-coupled, testable, and maintainable applications.

## Requiraments

-This is an application where you should record exercise data.

-You should choose one type of exercise only. We want to keep the app simple so you focus on the subject you're learning and not on the business complexities.

-You can choose raw SQL or Entity Framework for your data-persistence.

-The model for your exercise class should have at least the following properties: {Id INT, DateStart DateTime, DateEnd DateTime, Duration TimeSpan, Comments string}

-Your application should have the following classes: UserInput, ExerciseController, ExerciseService (where business logic will be handled) and ExerciseRepository. These classes might feel empty at first but they'll be needed in most applications as they grow.

-You can choose between SQLite or SQLServer.

-You need to use dependency injection to access the repository from the controller.


## Technologies Used

- **C#**
- **Entity Framework**
- **SQLServer**
- **Spectre.Console** (for console UI)
- **Repository Pattern**
- **Dependency Injection**

## Challenges

- It was hard to understand Dependency Injection


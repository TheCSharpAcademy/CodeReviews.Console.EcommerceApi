#Exercise Tracker
### Introduction
So far we have been developing our applications without thinking too much about their design. In software development, 
we’ll often use “general repeatable solutions to commonly occurring problems”, the so called design patterns.

We will build an exercise tracker using the “Repository Pattern”, an almost universally used solution for data persistence. 
It creates a layer between business logic and data access, which helps us create more loosely-coupled, testable and maintainable applications. 
You’ll be dealing with repositories on a daily-basis when you get your C# job!
### Requirements
 - [x] This is an application where you should record exercise data.
 - [x] You should choose one type of exercise only. We want to keep the app simple so you focus on the subject you're learning and not on the business complexities.
 - [x] You can choose raw SQL or Entity Framework for your data-persistence.
 - [x] The model for your exercise class should have at least the following properties: {Id INT, DateStart DateTime, DateEnd DateTime, Duration TimeSpan, Comments string}
 - [x] Your application should have the following classes: UserInput, ExerciseController, ExerciseService (where business logic will be handled) and ExerciseRepository. These classes might feel empty at first but they'll be needed in most applications as they grow.
 - [x] You can choose between SQLite or SQLServer.
 - [x] You need to use dependency injection to access the repository from the controller.
### Resources
[Microsoft](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=visual-studio)

[Implementing the Repository Pattern in C# and .NET](https://medium.com/@kerimkkara/implementing-the-repository-pattern-in-c-and-net-5fdd91950485)

[Repository Pattern in .NET Core](https://programmingwithwolfgang.com/repository-pattern-net-core/)

[Tutorial: Use dependency injection in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage)


### YouTube Videos
[ASP.NET Core Web API Repository Pattern Using Entity Framework Core DBContext | C# And .NET 8](https://www.youtube.com/watch?v=shzPIfZ70Pw)

[How to use the Repository Design Pattern in C# and ASP.NET](https://www.youtube.com/watch?v=8fFBWmbUaIg&t=994s)

[C# Interfaces Explained in Simple Terms | Mosh](https://www.youtube.com/watch?v=aQ8YkJrAbzE)

[Interfaces in C# - What they are, how to use them, and why they are so powerful](https://www.youtube.com/watch?v=A7qwuFnyIpM)

### Chalanges

This was a particurlay challanging assignment for me.  I hadn't worked with interfaces before and I had to bootstrap my knowlege up. 



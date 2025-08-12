# ExerciseTracker

Console based CRUD application to record exercise data. Developed using C# and SQLite, Entity Framework Core and Dependency Injection.

## Given Requirements
- [x] This is an application where you should record exercise data.
- [x] You should choose one type of exercise only. We want to keep the app simple so you focus on the subject you're learning and not on the business complexities.
- [x] You can choose raw SQL or Entity Framework for your data-persistence.
- [x] he model for your exercise class should have at least the following properties: {Id INT, DateStart DateTime, DateEnd DateTime, Duration TimeSpan, Comments string}
- [x] Your application should have the following classes: UserInput, ExerciseController, ExerciseService (where business logic will be handled) and ExerciseRepository.
- [x] You can choose between SQLite or SQLServer.
- [x] You need to use dependency injection to access the repository from the controller.
    
## Things I learnt
* Entity Framework
* SOLID
* Dependency Injection

## Resources
* [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)
* [Repository Pattern Docs](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* [Repository Pattern Tutorial](https://www.thecsharpacademy.com/project/18#:~:text=Repository%20Pattern%20Tutorial)

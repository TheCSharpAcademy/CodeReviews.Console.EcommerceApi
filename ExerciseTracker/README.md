# Exercise Tracker project by C# Academy

## Project Overview:
This is an application where you should record exercise data. Implementation should be done using Repository pattern

Project link: [Exercise Tracker](https://www.thecsharpacademy.com/project/18/exercise-tracker)

## Project Requirements:
- This is an application where you should record exercise data.
- You should choose one type of exercise only. We want to keep the app simple so you focus on the subject you're learning and not on the business complexities.
- You can choose raw SQL or Entity Framework for your data-persistence.
- The model for your exercise class should have at least the following properties: {Id INT, DateStart DateTime, DateEnd DateTime, Duration TimeSpan, Comments string}
- Your application should have the following classes: UserInput, ExerciseController, ExerciseService (where business logic will be handled) and ExerciseRepository. These classes might feel empty at first but they'll be needed in most applications as they grow.
- You can choose between SQLite or SQLServer.
- You need to use dependency injection to access the repository from the controller.

## Lessons Learned:
I was already quite comfortable with the repository pattern so base project was actually motivational boost I didn't know I needed. 
After feeling stuck on multiple projects, completing this one fairly quickly—and largely from memory—was a confidence boost and confirmation of progress.

Challenges were bit different story. Refactored the solution to introduce DTOs, which also required adjusting the service layer which helped me with implementation of the challenges 

The Dapper repository was bit tricky but I made it down quicker than expected. Had to refrest SQL joins and only showed me how simpler can things be with EF core.
For the first challenge I just re-used the same service and app was up and running.
The second challenge was harder. I wanted to have two services running - one for weight and one for cardio repository. The tricky part for me was actually the DI part as both of the services were implementing same inteface. I overcame this by implementing 2 new blank interfaces - IWeightExerciseService, ICardioExerciseService which were implementing base IService. The Service is implementing both of these interfaces. Not sure if this is the best long-term approach, but it works well for this project.

## Areas for improvement:

- Seed Data: Currently deserialized twice (once in DbContext, once in the Dapper repository). Could be split into separate files for better efficiency.
- Redundant Repository Logic: Both repositories implement the same method to retrieve exercise types. A dedicated ExerciseTypeRepository might be more elegant—but could add unnecessary complexity for the current scope.
- Hardcoded Service Selection: ProcessSelectExerciseType() in the controller uses hardcoded exercise IDs. Since exercise types aren't expected to change, this approach is acceptable for now.

## Main resources used:
Dapper Documentation 

## Packages Used

| Package | Version |
|---------|---------|
| Microsoft.Extensions.Configuration.UserSecrets | 6.0.1 |
| Spectre.Console | 0.50.0 |
| Microsoft.EntityFrameworkCore | 9.0.4 |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.4 |
| Microsoft.Data.SqlClient | 6.0.2 |
| Microsoft.EntityFrameworkCore.Tools | 9.0.4 |
| Dapper | 2.1.66 |
| Microsoft.EntityFrameworkCore.Design | 9.0.4 |

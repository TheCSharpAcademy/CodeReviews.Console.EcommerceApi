using Exercise_Tracker.Controllers;
using Exercise_Tracker.Data;
using Exercise_Tracker.Repository;
using Exercise_Tracker.Services;
using Exercise_Tracker.Views;

ExerciseDbContext dbContext = new ExerciseDbContext();

dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

Menu menu = new Menu(
    new ExerciseController(new ExerciseService(new ExerciseRepository(dbContext)))
);

menu.MainMenu();

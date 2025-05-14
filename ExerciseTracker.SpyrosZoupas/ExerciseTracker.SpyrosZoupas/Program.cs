using ExerciseTracker.SpyrosZoupas;
using ExerciseTracker.SpyrosZoupas.DAL;
using ExerciseTracker.SpyrosZoupas.DAL.Repository;

ExerciseTrackerDbContext dbContext = new ExerciseTrackerDbContext();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

IRepository<Exercise> exerciseRepository = new ExerciseRepository<Exercise>(dbContext);
ExerciseController exerciseController = new ExerciseController(exerciseRepository);
ExerciseService exerciseService = new ExerciseService(exerciseController);
UserInput userInput = new UserInput(exerciseService);
userInput.ExercisesMenu();
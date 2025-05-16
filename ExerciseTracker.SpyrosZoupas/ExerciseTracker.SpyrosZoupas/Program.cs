using ExerciseTracker.SpyrosZoupas;
using ExerciseTracker.SpyrosZoupas.Controller;
using ExerciseTracker.SpyrosZoupas.DAL;
using ExerciseTracker.SpyrosZoupas.DAL.Model;
using ExerciseTracker.SpyrosZoupas.DAL.Repository;
using ExerciseTracker.SpyrosZoupas.Services;

ExerciseTrackerDbContext dbContext = new ExerciseTrackerDbContext();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

IRepository<WeightExercise> weightExerciseRepository = new WeightExerciseRepository<WeightExercise>(dbContext);
IRepositoryDapper<CardioExercise> cardioExerciseRepository = new CardioExerciseRepository<CardioExercise>();
cardioExerciseRepository.CreateTables();

WeightExerciseController weightExerciseController = new WeightExerciseController(weightExerciseRepository);
CardioExerciseController cardioExerciseController = new CardioExerciseController(cardioExerciseRepository);

WeightExerciseService weightExerciseService = new WeightExerciseService(weightExerciseController);
CardioExerciseService cardioExerciseService = new CardioExerciseService(cardioExerciseController);


UserInput userInput = new UserInput(weightExerciseService, cardioExerciseService);
userInput.MainMenu();
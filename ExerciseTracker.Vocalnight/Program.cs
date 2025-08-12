using Exercise_Tracker;
using Exercise_Tracker.Controller;
using Exercise_Tracker.Model;
using Exercise_Tracker.Repository;
using Exercise_Tracker.Services;


//Instance of the context db
var dbContenxt = new ExerciseTrackerContext();

// Repository connects with the generic context of whichever db you are using
var repository = new ExcerciseRepository(dbContenxt);

//Service connects to the repository and receives data from controller.
var service = new ExcerciseService(repository);
var controller = new ExcerciseController(service);

//Insert the controller into the view
var view = new UserInput(controller);

view.Run();



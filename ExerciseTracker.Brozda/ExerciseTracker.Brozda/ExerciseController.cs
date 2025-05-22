using ExerciseTracker.Brozda.Helpers;
using ExerciseTracker.Brozda.Services;
using ExerciseTracker.Brozda.Services.Interfaces;
using ExerciseTracker.Brozda.UserInteraction;

namespace ExerciseTracker.Brozda
{
    /// <summary>
    /// Constrols flow of the application.
    /// Presents options to the user and performs requested actions
    /// </summary>
    internal class ExerciseController
    {
        /// <summary>
        /// Enum defining menu options and values
        /// </summary>
        private enum MenuOptions
        {
            ViewAll = 1,
            CreateRecord,
            EditRecord,
            DeleteRecord,
            ExitApp = 100,
        }

        /// <summary>
        /// A Map of menu options to label - text defining the option and <see cref="Func<<see cref="Task"/>>"/> defining an action to be performed for each option
        /// </summary>
        private readonly Dictionary<int, (string label, Func<Task> action)> _menuOptions = new Dictionary<int, (string label, Func<Task> action)>();

        private readonly IExerciseService _service;
        private readonly IUserInputOutput _ui;

        /// <summary>
        /// Initializes new instance of <see cref="ExerciseController"/>
        /// </summary>
        /// <param name="ui">A <see cref="UserInputOutput"/> handling Input and ouput actions to UI</param>
        /// <param name="service">A <see cref="ExerciseService"/> handling database access</param>
        public ExerciseController(IUserInputOutput ui, IExerciseService service)
        {
            _service = service;
            _ui = ui;
            MapMenu();
        }
        /// <summary>
        /// Maps enum representing option to respective label and action
        /// </summary>
        private void MapMenu()
        {
            _menuOptions.Add((int)MenuOptions.ViewAll, (AppStrings.ControllerViewAll, ProcessViewAll));
            _menuOptions.Add((int)MenuOptions.CreateRecord, (AppStrings.ControllerCreate, ProcessCreate));
            _menuOptions.Add((int)MenuOptions.EditRecord, (AppStrings.ControllerEdit, ProcessUpdate));
            _menuOptions.Add((int)MenuOptions.DeleteRecord, (AppStrings.ControllerDelete, ProcessDelete));
            _menuOptions.Add((int)MenuOptions.ExitApp, (AppStrings.ControllerExit, ProcessExitApp));



        }

        /// <summary>
        /// Initializes the app flow. Which is ongoing until user decides to exit the application
        /// </summary>
        public async Task Run()
        {
            int menuChoice = _ui.ShowMenuAndGetInput(_menuOptions.ToDictionary(x => x.Key, x=> x.Value.label));

            while(menuChoice != (int)MenuOptions.ExitApp)
            {
                if(_menuOptions.TryGetValue(menuChoice, out var labelActionPair))
                {
                    await labelActionPair.action();
                    _ui.PrintPressAnyKeyToContinue();

                    menuChoice = _ui.ShowMenuAndGetInput(_menuOptions.ToDictionary(x => x.Key, x => x.Value.label));
                }
            }
        }
        /// <summary>
        /// Gets all existing exercises from repository and prints them to user output
        /// User is informed about any error
        /// </summary>
        public async Task ProcessViewAll()
        {
            var getAllResult = await _service.ViewAllAsync();

            if (getAllResult.IsSucessul && getAllResult.Data is not null)
            {
                _ui.PrintExercises(getAllResult.Data);
            }
            else
            {
                _ui.PrintError(getAllResult.ErrorMessage);
            }
        }
        /// <summary>
        /// Gets data for new exercise from user input and inserts it into the database
        /// User is informed about any error
        /// </summary
        public async Task ProcessCreate()
        {
            var exTypes = await _service.GetExerciseTypes();
            var exercise = _ui.GetExercise(exTypes.Data!,null);
            

            var createResult = await _service.CreateAsync(exercise);

            if (createResult.IsSucessul && createResult.Data is not null)
            {
                _ui.PrintText(AppStrings.ControllerSuccessCreate);
                _ui.PrintExercise(createResult.Data);
            }
            else
            {
                _ui.PrintError(createResult.ErrorMessage);
            }
        }
        /// <summary>
        /// Updates existing exercise base on data from user input and inserts it into the database
        /// User is informed about any error
        /// </summary
        public async Task ProcessUpdate()
        {
            int id = await GetIdFromUser();

            var exTypes = await _service.GetExerciseTypes();
            var getByIdResult = await _service.GetByIdAsync(id);

            if (!getByIdResult.IsSucessul || getByIdResult.Data is null)
            {
                _ui.PrintError(getByIdResult.ErrorMessage);
                return;
            }

            var updatedExercise = _ui.GetExercise(exTypes.Data!,getByIdResult.Data);
            updatedExercise.Id = getByIdResult.Data.Id; 

            var updateResult = await _service.EditAsync(id, updatedExercise);

            if (updateResult.IsSucessul && updateResult.Data is not null)
            {
                _ui.PrintText(AppStrings.ControllerSuccessEdit);
                _ui.PrintExercise(updateResult.Data);
            }
            else
            {
                _ui.PrintError(updateResult.ErrorMessage);
            }

        }
        /// <summary>
        /// Deletes existing exercise from database
        /// </summary>
        public async Task ProcessDelete()
        {
            int id = await GetIdFromUser();

            var deleteResult = await _service.DeleteAsync(id);

            if (deleteResult.IsSucessul && deleteResult.Data is true)
            {
                _ui.PrintText(AppStrings.ControllerSuccessDelete);
            }
            else
            {
                _ui.PrintError(deleteResult.ErrorMessage);
            }

        }
        /// <summary>
        /// Exits the application
        /// </summary>
        private Task ProcessExitApp()
        {
            Environment.Exit(0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets an Id of existing records from user input
        /// </summary>
        /// <returns>A task result contains <see cref="int"/> representing the record Id</returns>
        private async Task<int> GetIdFromUser()
        {
            var getAllResult = await _service.ViewAllAsync();

            if (!getAllResult.IsSucessul || getAllResult.Data is null)
            {
                _ui.PrintError(getAllResult.ErrorMessage);
                return 0;
            }

            return _ui.GetRecordId(getAllResult.Data);
        }
        
    }
}

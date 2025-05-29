using ExerciseTracker.Brozda.Helpers;
using ExerciseTracker.Brozda.Models;
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
            SelectExerciseType = 100,
            ExitApp = 101,
        }

        private readonly Dictionary<int, string> _menuOptions = new Dictionary<int, string>();
        private readonly Dictionary<int, Func<Task>> _menuActions = new Dictionary<int, Func<Task>>();

        private readonly IWeightExerciseService _weightExerciseService;
        private readonly ICardioExerciseService _cardioExerciseService;
        private readonly IUserInputOutput _ui;

        private IExerciseService? _activeService;
        private ExerciseType? _activeExerciseType;

        /// <summary>
        /// Initializes new instance of <see cref="ExerciseController"/>
        /// </summary>
        /// <param name="ui">A <see cref="UserInputOutput"/> handling Input and ouput actions to UI</param>
        /// <param name="service">A <see cref="ExerciseService"/> handling database access</param>
        public ExerciseController(IUserInputOutput ui, IWeightExerciseService weightExerciseService, ICardioExerciseService cardioExerciseService)
        {
            _weightExerciseService = weightExerciseService;
            _cardioExerciseService = cardioExerciseService;
            _ui = ui;
            MapMenu();
        }

        /// <summary>
        /// Maps enum representing option to respective label and action
        /// </summary>
        private void MapMenu()
        {
            //This could be Dictionary <int, (string, Func<Task>)> but I found it less readable

            _menuOptions.Add((int)MenuOptions.ViewAll, AppStrings.ControllerViewAll);
            _menuOptions.Add((int)MenuOptions.CreateRecord, AppStrings.ControllerCreate);
            _menuOptions.Add((int)MenuOptions.EditRecord, AppStrings.ControllerEdit);
            _menuOptions.Add((int)MenuOptions.DeleteRecord, AppStrings.ControllerDelete);
            _menuOptions.Add((int)MenuOptions.SelectExerciseType, AppStrings.SelectExerciseType);
            _menuOptions.Add((int)MenuOptions.ExitApp, AppStrings.ControllerExit);

            _menuActions.Add((int)MenuOptions.ViewAll, ProcessViewAll);
            _menuActions.Add((int)MenuOptions.CreateRecord, ProcessCreate);
            _menuActions.Add((int)MenuOptions.EditRecord, ProcessUpdate);
            _menuActions.Add((int)MenuOptions.DeleteRecord, ProcessDelete);
            _menuActions.Add((int)MenuOptions.SelectExerciseType, ProcessSelectExerciseType);
            _menuActions.Add((int)MenuOptions.ExitApp, ProcessExitApp);
        }

        /// <summary>
        /// Sets activeService and activeExercise type used to DB access and data modification
        /// </summary>
        /// <returns></returns>
        private async Task ProcessSelectExerciseType()
        {
            var exTypes = await _weightExerciseService.GetExerciseTypes();

            _ui.PrintText(AppStrings.IoSelectDatabase);

            var exTypeId = _ui.GetExerciseTypeId(exTypes.Data!);

            _activeExerciseType = exTypes.Data!.First(x => x.Id == exTypeId);

            _ui.ClearConsole();

            _activeService = (exTypeId == 1)
                ? _weightExerciseService
                : _cardioExerciseService;
        }

        /// <summary>
        /// Initializes the app flow. Which is ongoing until user decides to exit the application
        /// </summary>
        public async Task Run()
        {
            await ProcessSelectExerciseType();

            int menuChoice = _ui.ShowMenuAndGetInput(_menuOptions);

            while (menuChoice != (int)MenuOptions.ExitApp)
            {
                if (_menuActions.TryGetValue(menuChoice, out var menuAction))
                {
                    await menuAction();
                }

                if (menuChoice != (int)MenuOptions.SelectExerciseType)
                {
                    _ui.PrintPressAnyKeyToContinue();
                }

                menuChoice = _ui.ShowMenuAndGetInput(_menuOptions);
            }
        }

        /// <summary>
        /// Gets all existing exercises from repository and prints them to user output
        /// User is informed about any error
        /// </summary>
        public async Task ProcessViewAll()
        {
            if (_activeService is null)
            {
                _ui.PrintText(AppStrings.IoDatabaseNotSelected);
                return;
            }

            var getAllResult = await _activeService.ViewAllAsync();

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
            if (_activeService is null || _activeExerciseType is null)
            {
                _ui.PrintText(AppStrings.IoDatabaseNotSelected);
                return;
            }

            var exercise = _ui.GetExercise(_activeExerciseType, null);

            var createResult = await _activeService.CreateAsync(exercise);

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
            if (_activeService is null || _activeExerciseType is null)
            {
                _ui.PrintText(AppStrings.IoDatabaseNotSelected);
                return;
            }

            int id = await GetIdFromUser();

            var exTypes = await _activeService.GetExerciseTypes();

            var getByIdResult = await _activeService.GetByIdAsync(id);

            if (!getByIdResult.IsSucessul || getByIdResult.Data is null)
            {
                _ui.PrintError(getByIdResult.ErrorMessage);
                return;
            }

            var updatedExercise = _ui.GetExercise(_activeExerciseType, getByIdResult.Data);
            updatedExercise.Id = getByIdResult.Data.Id;

            var updateResult = await _activeService.EditAsync(id, updatedExercise);

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
            if (_activeService is null || _activeExerciseType is null)
            {
                _ui.PrintText(AppStrings.IoDatabaseNotSelected);
                return;
            }

            int id = await GetIdFromUser();

            var deleteResult = await _activeService.DeleteAsync(id);

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
            if (_activeService is null || _activeExerciseType is null)
            {
                _ui.PrintText(AppStrings.IoDatabaseNotSelected);
                return 0;
            }

            var getAllResult = await _activeService.ViewAllAsync();

            if (!getAllResult.IsSucessul || getAllResult.Data is null)
            {
                _ui.PrintError(getAllResult.ErrorMessage);
                return 0;
            }

            return _ui.GetRecordId(getAllResult.Data);
        }
    }
}
using ExerciseTracker.Brozda.Services;
using ExerciseTracker.Brozda.UserInteraction;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace ExerciseTracker.Brozda
{
    internal class ExerciseController
    {
        private enum MenuOptions
        {
            ViewAll = 0,
            ViewById,
            CreateRecord,
            EditRecord,
            DeleteRecord,
            ExitApp,
        }

        private readonly Dictionary<int, (string label, Func<Task> action)> _menuOptions = new Dictionary<int, (string label, Func<Task> action)>();

        private readonly ExerciseService _service;
        private readonly UserInput _ui;
        public ExerciseController(UserInput ui, ExerciseService service)
        {
            _service = service;
            _ui = ui;
            MapMenu();
        }
        private void MapMenu()
        {
            _menuOptions.Add((int)MenuOptions.ViewAll, ("View all excercises", ProcessViewAll));
            _menuOptions.Add((int)MenuOptions.ViewById, ("View specific excercise using its Id", ProcessById));
            _menuOptions.Add((int)MenuOptions.CreateRecord, ("Create a new excercise", ProcessCreate));
            _menuOptions.Add((int)MenuOptions.EditRecord, ("Update existing excercise", ProcessUpdate));
            _menuOptions.Add((int)MenuOptions.DeleteRecord, ("Delete existing excercise", ProcessDelete));
            _menuOptions.Add((int)MenuOptions.ExitApp, ("Exit the application", ProcessExitApp));



        }
        public async void Run()
        {
            int menuChoice = _ui.ShowMenuAndGetInput(_menuOptions.ToDictionary(x => x.Key, x=> x.Value.label));

            while(menuChoice != (int)MenuOptions.ExitApp)
            {
                if(_menuOptions.TryGetValue(menuChoice, out var labelActionPair))
                {
                    await labelActionPair.action();


                    menuChoice = _ui.ShowMenuAndGetInput(_menuOptions.ToDictionary(x => x.Key, x => x.Value.label));
                }
            }
        }
        public async Task ProcessViewAll()
        {
            var getAllResult = await _service.ViewAllAsync();

            if (getAllResult.IsSucessul && getAllResult.Data is not null)
            {
                _ui.PrintExercises(getAllResult.Data);
            }
            else
            {
                _ui.PrintError(getAllResult.ErrorMessage ?? "Unhandled error");
            }
        }
        public async Task ProcessById()
        {
            int id = await GetIdFromUser();

            var getByIdResult = await _service.GetByIdAsync(id);

            if (getByIdResult.IsSucessul && getByIdResult.Data is not null)
            {
                _ui.PrintExercise(getByIdResult.Data);
            }
            else
            {
                _ui.PrintError(getByIdResult.ErrorMessage ?? "Unhandled error");
            }
        }
        public async Task ProcessCreate()
        {
            var exercise = _ui.GetExercise(null);

            var createResult = await _service.CreateAsync(exercise);

            if (createResult.IsSucessul && createResult.Data is not null)
            {
                _ui.PrintText("Exercise added successfully");
                _ui.PrintExercise(createResult.Data);
            }
            else
            {
                _ui.PrintError(createResult.ErrorMessage ?? "Unhandled error");
            }
        }
        public async Task ProcessUpdate()
        {
            int id = await GetIdFromUser();

            var getByIdResult = await _service.GetByIdAsync(id);

            if (!getByIdResult.IsSucessul || getByIdResult.Data is null)
            {
                _ui.PrintError(getByIdResult.ErrorMessage ?? "Unhandled error");
                return;
            }

            var updatedExercise = _ui.GetExercise(getByIdResult.Data);
            updatedExercise.Id = getByIdResult.Data.Id; 

            var updateResult = await _service.EditAsync(id, updatedExercise);

            if (updateResult.IsSucessul && updateResult.Data is not null)
            {
                _ui.PrintText("Exercise updated successfully");
                _ui.PrintExercise(updateResult.Data);
            }
            else
            {
                _ui.PrintError(updateResult.ErrorMessage ?? "Unhandled error");
            }

        }
        public async Task ProcessDelete()
        {
            int id = await GetIdFromUser();

            var deleteResult = await _service.DeleteAsync(id);

            if (deleteResult.IsSucessul && deleteResult.Data is true)
            {
                _ui.PrintText("Exercise deleted successfully");
            }
            else
            {
                _ui.PrintError(deleteResult.ErrorMessage ?? "Unhandled error");
            }

        }
        private Task ProcessExitApp()
        {
            Environment.Exit(0);
            return Task.CompletedTask;
        }


        private async Task<int> GetIdFromUser()
        {
            var getAllResult = await _service.ViewAllAsync();

            if (!getAllResult.IsSucessul || getAllResult.Data is null)
            {
                _ui.PrintError(getAllResult.ErrorMessage ?? "Unhandled error");
                return 0;
            }

            return _ui.GetRecordId(getAllResult.Data, "Please select id of exercise you'd like to see: ");
        }
        
    }
}

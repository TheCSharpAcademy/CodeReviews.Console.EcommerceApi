using Exercise_Tracker.Interfaces;
using Exercise_Tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tracker.Services
{
    internal class ExcerciseService
    {
        private readonly IExcerciseRepository exerciseRepository;

        public ExcerciseService(IExcerciseRepository exerciseRepository )
        {
            this.exerciseRepository = exerciseRepository;
        }

        public void AddExercise(Exercise exercise)
        {
            if (exercise != null)
            {
                exerciseRepository.AddRegistry(exercise);
            }
        }

        public IEnumerable<Exercise> GetExercises()
        {
            return exerciseRepository.GetAll();
        }

        public void RemoveRegistry(int id)
        {
            List<Exercise> exercises = (List<Exercise>)GetExercises();
            
            if (exercises.Where(e => e.Id == id).Count() == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Id not valid. Try again\n");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            exerciseRepository.RemoveRegistry(id);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nRegistry removed!\n");
            Console.ForegroundColor = ConsoleColor.White;
        } 
    }
}

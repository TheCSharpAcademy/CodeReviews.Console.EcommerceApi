using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTracker.JsPeanut
{
    public interface IExerciseService
    {
        IEnumerable<Exercise> GetExercises();

        Exercise Get(int id);

        void Insert(Exercise exercise);

        void Delete(int id);

        void Update(Exercise exercise);

        void Save();
    }
}

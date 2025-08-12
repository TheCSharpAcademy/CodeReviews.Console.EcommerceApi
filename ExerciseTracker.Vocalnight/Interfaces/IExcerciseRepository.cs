using Exercise_Tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tracker.Interfaces
{
    public interface IExcerciseRepository
    {
        Exercise SearchById( int id );

        IEnumerable<Exercise> GetAll();

        void AddRegistry( Exercise exercise );
        void RemoveRegistry( int id );

        void UpdateRegistry( Exercise exercise );
    }
}

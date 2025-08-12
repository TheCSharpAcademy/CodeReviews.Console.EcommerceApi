using ExerciseUI.Model;

namespace ExerciseUI.Repositories
{
    public interface IRepository<T>
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        bool Create(T entity);
        bool Delete(int id);
        bool Update(T entity);
    }
    public class ExerciseRepository<T> : IRepository<T> where T : class
    {
        private readonly ExerciseContext _context;

        public ExerciseRepository(ExerciseContext context)
        {
            _context = context;
        }
        
        public bool Create(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Error occured while adding an exercise.");
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var item = _context.Set<T>().Find(id);
                _context.Set<T>().Remove(item);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Error occured while deleting an exercise.");
            }
        }

        public T Get(int id)
        {
            try
            {
                return _context.Set<T>().Find(id);
            }
            catch
            {
                throw new Exception("Error occured while fetching an exercise.");
            }
        }

        public IEnumerable<T> GetAll() 
        {
            try
            {
                return _context.Set<T>();
            }
            catch
            {
                throw new Exception("Error occured while fetching exercises.");
            }
        }

        public bool Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Error occured while updating an exercise.");
            }
        }
    }
}

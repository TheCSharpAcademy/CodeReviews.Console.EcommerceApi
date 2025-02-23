using exerciseTracker.doc415.Controller;

namespace exerciseTracker.doc415
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var _controller = new ExerciseController();
            _controller.MainMenu();
        }
    }
}

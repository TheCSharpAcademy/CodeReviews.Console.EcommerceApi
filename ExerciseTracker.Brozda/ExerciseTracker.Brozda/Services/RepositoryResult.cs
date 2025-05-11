namespace ExerciseTracker.Brozda.Services
{
    internal class RepositoryResult<T>
    {
        public bool IsSucessul { get; set; }
        public T? Data { get; set; }

        public string? ErrorMessage;

        public static RepositoryResult<T> Success(T data) 
        {
            return new RepositoryResult<T>
            {
                Data = data,
                IsSucessul = true
            };
        }
        public static RepositoryResult<T> Fail(string errorMsg)
        {
            return new RepositoryResult<T>
            {
                IsSucessul = false,
                ErrorMessage = errorMsg
            };
        }
        public static RepositoryResult<T> NotFound()
        {
            return new RepositoryResult<T>
            {
                IsSucessul = false,
                ErrorMessage = "Requested record not found"
            };
        }
    }
}

namespace ExerciseTracker.Brozda.Models
{
    /// <summary>
    /// Represent result of database call result
    /// Contains confirmation if the call was successful and may contain retrieved data and error message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class RepositoryResult<T>
    {
        public bool IsSucessul { get; set; }
        public T? Data { get; set; }

        public string? ErrorMessage;

        /// <summary>
        /// Returns a successful <see cref="RepositoryResult{T}"/> containing the provided data.
        /// </summary>
        /// <param name="data">The data to include in the result..</param>
        /// <returns>A successful result with the given data</returns>
        public static RepositoryResult<T> Success(T data)
        {
            return new RepositoryResult<T>
            {
                Data = data,
                IsSucessul = true
            };
        }

        /// <summary>
        /// Returns a failed <see cref="RepositoryResult{T}"/> containing the provided error message
        /// </summary>
        /// <param name="errorMsg">Error message describing the failure</param>
        /// <returns>Failed result contaning provided error message</returns>
        public static RepositoryResult<T> Fail(string errorMsg)
        {
            return new RepositoryResult<T>
            {
                IsSucessul = false,
                ErrorMessage = errorMsg
            };
        }

        /// <summary>
        /// Returns a failed <see cref="RepositoryResult{T}"/> indicating that requested data were not found
        /// </summary>
        /// <returns>Failed result indicating that requested data were not found</returns>
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
namespace ExerciseTracker.Brozda.Models
{
    internal static class RepositoryResultExtensions
    {
        /// <summary>
        /// Re-map <see cref="RepositoryResult{T}"/> from DTO to model and vice versa
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="input"></param>
        /// <param name="mapFunc"></param>
        /// <returns></returns>
        internal static RepositoryResult<TOut> Map<TIn, TOut>(
            this RepositoryResult<TIn> input,
            Func<TIn, TOut> mapFunc)
        {
            if (!input.IsSucessul)
                return RepositoryResult<TOut>.Fail(input.ErrorMessage);

            if (input.Data == null)
                return RepositoryResult<TOut>.NotFound();

            return RepositoryResult<TOut>.Success(mapFunc(input.Data));
        }
    }
}

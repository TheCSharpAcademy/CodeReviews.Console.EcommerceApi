using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTracker.Brozda.Models
{
    internal static class RepositoryResultExtensions
    {
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

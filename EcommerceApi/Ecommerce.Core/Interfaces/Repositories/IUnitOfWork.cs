using Ecommerce.Core.Models;

namespace Ecommerce.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    ISaleRepository Sales { get; }

    Task<int> CompleteAsync();
    void Dispose();
}
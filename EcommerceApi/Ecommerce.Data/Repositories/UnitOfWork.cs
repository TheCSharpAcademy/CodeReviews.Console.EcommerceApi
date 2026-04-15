using Ecommerce.Core.Interfaces.Repositories;

namespace Ecommerce.Data.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IProductRepository? _products;
    private ICategoryRepository? _categories;
    private ISaleRepository? _sales;
    public IProductRepository Products => _products ??= new ProductRepository(context);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(context);
    public ISaleRepository Sales => _sales ??= new SaleRepository(context);
    public async Task<int> CompleteAsync()
    {
       return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
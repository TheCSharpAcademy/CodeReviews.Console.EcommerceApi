using Ecommerce.Core.Interfaces.Repositories;
using Ecommerce.Core.Models;

namespace Ecommerce.Data.Repositories;

public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
{
}
using Ecommerce.Core.Interfaces.Repositories;
using Ecommerce.Core.Models;

namespace Ecommerce.Data.Repositories;

public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
{
}
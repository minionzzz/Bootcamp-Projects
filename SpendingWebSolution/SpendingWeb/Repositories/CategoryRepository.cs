using SpendingWeb.Models;

namespace SpendingWeb.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(SpendSmartDbContext context) : base(context) { }
    }
}
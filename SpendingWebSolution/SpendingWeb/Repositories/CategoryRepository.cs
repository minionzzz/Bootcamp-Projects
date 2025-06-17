using SpendingWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace SpendingWeb.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private new readonly SpendSmartDbContext _context;

        public CategoryRepository(SpendSmartDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategoriesWithExpenses()
        {
            return _context.Categories
                .Include(c => c.Expenses)
                .ToList();
        }

        public Category? GetCategoryWithExpenses(int id) // Add nullable
        {
            return _context.Categories
                .Include(c => c.Expenses)
                .FirstOrDefault(c => c.Id == id);
        }

        public Category? GetCategoryByName(string name) // Add nullable
        {
            return _context.Categories
                .FirstOrDefault(c => c.Name == name);
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

    }
}
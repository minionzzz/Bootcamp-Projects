using SpendingWeb.Models;

namespace SpendingWeb.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category? GetCategoryByName(string name); // Make nullable
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
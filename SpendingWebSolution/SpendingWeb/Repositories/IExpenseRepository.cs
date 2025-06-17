using SpendingWeb.Models;

namespace SpendingWeb.Repositories
{
    public interface IExpenseRepository : IRepository<Expense>
    { 
    // Method khusus untuk Expense
    IEnumerable<Expense> GetExpensesByDateRange(DateTime start, DateTime end);
    IEnumerable<Expense> GetExpensesByCategory(int categoryId);
    decimal GetTotalExpenses(DateTime month);
    IEnumerable<Expense> GetRecentExpenses(int count);
    }
}
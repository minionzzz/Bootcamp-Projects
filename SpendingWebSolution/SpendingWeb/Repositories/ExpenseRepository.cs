using SpendingWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace SpendingWeb.Repositories
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        private new readonly SpendSmartDbContext _context;

        public ExpenseRepository(SpendSmartDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Expense> GetExpensesByDateRange(DateTime start, DateTime end)
        {
            return _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.Date >= start && e.Date <= end)
                .ToList();
        }

        public IEnumerable<Expense> GetExpensesByCategory(int categoryId)
        {
            return _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.CategoryId == categoryId)
                .ToList();
        }

        public decimal GetTotalExpenses(DateTime month)
        {
            return _context.Expenses
                .Where(e => e.Date.Month == month.Month && e.Date.Year == month.Year)
                .Sum(e => e.Amount);
        }

        public IEnumerable<Expense> GetRecentExpenses(int count)
        {
            return _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .Take(count)
                .ToList();
        }
    }
}
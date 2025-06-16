using SpendingWeb.Models;

namespace SpendingWeb.Repositories
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(SpendSmartDbContext context) : base(context) { }
    }
}
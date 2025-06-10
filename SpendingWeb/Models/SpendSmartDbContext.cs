namespace SpendingWeb.Models;

using Microsoft.EntityFrameworkCore;

public class SpendSmartDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; } = null!;
    public SpendSmartDbContext(DbContextOptions<SpendSmartDbContext> options)
        : base(options)
    {
        
    }
}
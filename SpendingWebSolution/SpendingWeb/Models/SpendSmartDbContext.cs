namespace SpendingWeb.Models;

using Microsoft.EntityFrameworkCore;

public class SpendSmartDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public SpendSmartDbContext(DbContextOptions<SpendSmartDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relasi: Expense - Category (banyak ke satu)
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull); // Jika kategori dihapus, CategoryId di Expense jadi null

        base.OnModelCreating(modelBuilder);
    }
}
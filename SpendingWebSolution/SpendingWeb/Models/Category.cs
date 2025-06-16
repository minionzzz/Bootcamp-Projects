using System.ComponentModel.DataAnnotations;

namespace SpendingWeb.Models;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhotoPath{ get; set; }
    public ICollection<Expense>? Expenses { get; set; }
}
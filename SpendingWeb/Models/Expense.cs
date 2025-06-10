using System.ComponentModel.DataAnnotations;

namespace SpendingWeb.Models;

public class Expense
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    [Required]
    public string? Description { get; set; }
    
}
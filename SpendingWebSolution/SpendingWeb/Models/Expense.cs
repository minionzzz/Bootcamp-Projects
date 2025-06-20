using System.ComponentModel.DataAnnotations;

namespace SpendingWeb.Models;

public class Expense
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    [Required]
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? AttachmentPath { get; set; }    
}
namespace SpendingWeb.DTOs;

public class CreateExpenseDTO
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public int? CategoryId { get; set; }
    public string? AttachmentPath { get; set; }
}
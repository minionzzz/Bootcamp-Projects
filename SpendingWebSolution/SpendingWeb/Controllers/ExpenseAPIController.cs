using Microsoft.AspNetCore.Mvc;
using SpendingWeb.Services;
using SpendingWeb.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ExpensesApiController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesApiController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    // GET: api/ExpensesApi
    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var expenses = await _expenseService.GetAllAsync();
        return Ok(expenses);
    }

    // GET: api/ExpensesApi/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpense(int id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense == null)
            return NotFound();
        return Ok(expense);
    }

    // POST: api/ExpensesApi
    [HttpPost]
    public async Task<IActionResult> PostExpense([FromBody] CreateExpenseDTO createExpenseDTO)
    {
        var created = await _expenseService.CreateAsync(createExpenseDTO);
        return CreatedAtAction(nameof(GetExpense), new { id = created.Id }, created);
    }

    // PUT: api/ExpensesApi/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseDTO expenseDto)
    {
        var updated = await _expenseService.UpdateAsync(id, expenseDto);
        if (!updated)
            return NotFound();
        return NoContent();
    }

    // DELETE: api/ExpensesApi/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var deleted = await _expenseService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
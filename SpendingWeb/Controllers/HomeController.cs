using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendingWeb.Models;

namespace SpendingWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SpendSmartDbContext _context;

    public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Expense()
    {
        var allExpenses = _context.Expenses.ToList();

        var totalExpense = allExpenses.Sum(exp => exp.Amount);

        ViewBag.Expense = totalExpense;

        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? id)
    {
        if (id != null)
        {
            var expenseInDB = _context.Expenses.SingleOrDefault(exp => exp.Id == id.Value);
            return View(expenseInDB);
        }
        return View();
    }

    public IActionResult DeleteExpense(int id)
    {
        var expenseInDB = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
        if (expenseInDB != null)
        {
            _context.Expenses.Remove(expenseInDB);
            _context.SaveChanges();
        }
        return RedirectToAction("Expense");
    }

    public IActionResult ExpensesList(Expense model)
    {
        if (model.Id == 0)
        {
            _context.Expenses.Add(model);
        }
        else
        {
            _context.Expenses.Update(model);
        }
        
        _context.SaveChanges();
        return RedirectToAction("Expense");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

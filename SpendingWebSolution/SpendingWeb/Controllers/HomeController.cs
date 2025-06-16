using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendingWeb.Models;
using System.Text;

namespace SpendingWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SpendSmartDbContext _context;
    private const decimal MonthlyLimit = 2000; 

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
        
        var allExpenses = _context.Expenses.Include(e => e.Category).OrderByDescending(e => e.Date).ToList();

        var totalExpense = allExpenses.Sum(exp => exp.Amount);

        var now = DateTime.Now;
        var monthlyExpense = allExpenses
        .Where(e => e.Date.Month == now.Month && e.Date.Year == now.Year)
        .Sum(e => e.Amount);

        ViewBag.Expense = totalExpense;
        ViewBag.ShowReminder = monthlyExpense > MonthlyLimit;
        ViewBag.MonthlyExpense = monthlyExpense;
        ViewBag.MonthlyLimit = MonthlyLimit;

        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? id)
    {
        var expense = new Expense();
        if (id.HasValue)
        {
            expense = _context.Expenses.Include(e => e.Category).SingleOrDefault(e => e.Id == id.Value);
            if (expense == null)
            {
                return NotFound();
            }
        }
        ViewBag.Categories = _context.Categories.ToList();
        return View(expense);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateEditExpense(Expense expense, IFormFile? Attachment, string? ExistingAttachmentPath)
    {
        if (ModelState.IsValid)
        {
            if (Attachment != null && Attachment.Length > 0)
            {
                var fileName = Path.GetFileName(Attachment.FileName);
                var filePath = Path.Combine("wwwroot/attachments", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Attachment.CopyTo(stream);
                }
                expense.AttachmentPath = "/attachments/" + fileName;
            }
            else
            {
                expense.AttachmentPath = ExistingAttachmentPath;
            }

            if (expense.Id == 0)
            {
                _context.Expenses.Add(expense);
            }
            else
            {
                _context.Expenses.Update(expense);
            }
            _context.SaveChanges();
            return RedirectToAction("Expense");
        }
        return View(expense);
    }

    public IActionResult DeleteExpense(int id)
    {
        var expenseInDB = _context.Expenses.Include(e => e.Category).SingleOrDefault(expense => expense.Id == id);
        if (expenseInDB != null)
        {
            _context.Expenses.Remove(expenseInDB);
            _context.SaveChanges();
        }
        return RedirectToAction("Expense");
    }

     [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExpensesList(Expense expense, IFormFile? Attachment, string? ExistingAttachmentPath)
    {
        if (ModelState.IsValid)
        {
            // Handle file upload
            if (Attachment != null && Attachment.Length > 0)
            {
                var fileName = Path.GetFileName(Attachment.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "attachments");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Attachment.CopyTo(stream);
                }
                expense.AttachmentPath = "/attachments/" + fileName;
            }
            else
            {
                // Pakai attachment lama jika tidak upload baru
                expense.AttachmentPath = ExistingAttachmentPath;
            }

            if (expense.Id == 0)
            {
                _context.Expenses.Add(expense);
            }
            else
            {
                _context.Expenses.Update(expense);
            }
            _context.SaveChanges();
            return RedirectToAction("Expense");
        }

        // Jika gagal validasi, tampilkan kembali form
        ViewBag.Categories = _context.Categories.ToList();
        return View("CreateEditExpense", expense);
    }

    public IActionResult ExportExpenses()
    {
        var expenses = _context.Expenses.Include(e => e.Category).OrderByDescending(e => e.Date).ToList();

        var csv = new StringBuilder();
        csv.AppendLine("Amount,Description,Date,Category");

        foreach (var exp in expenses)
        {
            csv.AppendLine($"\"{exp.Amount}\",\"{exp.Description}\",\"{exp.Date:yyyy-MM-dd}\",\"{exp.Category?.Name}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "ExpenseData.csv");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

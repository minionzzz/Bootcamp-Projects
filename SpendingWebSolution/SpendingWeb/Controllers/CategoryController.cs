using Microsoft.AspNetCore.Mvc;
using SpendingWeb.Models;
using System.Linq;

public class CategoriesController : Controller
{
    private readonly SpendSmartDbContext _context;

    public CategoriesController(SpendSmartDbContext context)
    {
        _context = context;
    }

    // GET: Categories
    public IActionResult Index()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: Categories/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();
        var category = _context.Categories.Find(id);
        if (category == null) return NotFound();
        return View(category);
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Category category, IFormFile? Photo, string? ExistingPhotoPath)
    {
        if (id != category.Id) return NotFound();
        if (ModelState.IsValid)
        {
            if (Photo != null && Photo.Length > 0)
            {
                var fileName = Path.GetFileName(Photo.FileName);
                var filePath = Path.Combine("wwwroot/attachments", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(stream);
                }
                category.PhotoPath = "/attachments/" + fileName;
            }
            else
            {
                category.PhotoPath = ExistingPhotoPath;
            }
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: Categories/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();
        var category = _context.Categories.Find(id);
        if (category == null) return NotFound();
        return View(category);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int? id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        var relatedExpenses = _context.Expenses.Where(e => e.CategoryId == id).ToList();
        foreach (var exp in relatedExpenses)
        {
            exp.CategoryId = null; // atau exp.CategoryId = idUnknown jika pakai kategori "Unknown"
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
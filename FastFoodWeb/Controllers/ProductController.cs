using FastFoodWeb.Data;
using FastFoodWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFoodWeb.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductController(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }
    public async Task<IActionResult> Index(int? categoryId, string? keyword)
    {
        ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.CategoryId = categoryId;
        ViewBag.Keyword = keyword;
        var query = _db.Products.Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p => p.Name.Contains(keyword));
        }
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }
        var products = await query.OrderByDescending(p => p.Id).ToListAsync();
        return View(products);
    }
    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();
        var related = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == product.CategoryId
                     && p.Id != id
                     && p.IsAvailable)
            .Take(4)
            .ToListAsync();

        ViewBag.Related = related;

        return View(product);
    }
}
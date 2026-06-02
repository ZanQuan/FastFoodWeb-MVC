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
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            product.ImageUrl = await SaveImageAsync(imageFile);

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Đã thêm sản phẩm \"{product.Name}\"";

            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAsync(product.CategoryId);
        return View(product);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        await LoadCategoriesAsync(product.CategoryId);

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
    {
        if (id != product.Id)
            return NotFound();
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                product.ImageUrl = await SaveImageAsync(imageFile);
            }
            else
            {
                var existing = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                product.ImageUrl = existing?.ImageUrl;
            }
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Đã cập nhật \"{product.Name}\"";
            return RedirectToAction(nameof(Index));
        }
        await LoadCategoriesAsync(product.CategoryId);
        return View(product);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            DeleteImageFile(product.ImageUrl);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Đã xoá \"{product.Name}\"";
        }
        return RedirectToAction(nameof(Index));
    }
    private async Task<string?> SaveImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return null;
        string folder = Path.Combine(_env.WebRootPath,"images","products");
        Directory.CreateDirectory(folder);
        string fileName =Guid.NewGuid() +Path.GetExtension(file.FileName);
        string fullPath =Path.Combine(folder, fileName);
        await using var stream =new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        return "/images/products/" + fileName;
    }

    private void DeleteImageFile(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return;
        string fullPath = Path.Combine(_env.WebRootPath,imageUrl.TrimStart('/'));

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }
    private async Task LoadCategoriesAsync(int selectedId = 0)
    {
        var cats = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Categories =new SelectList(cats, "Id", "Name", selectedId);
    }
}
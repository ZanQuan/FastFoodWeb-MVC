using FastFoodWeb.Data;
using FastFoodWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFoodWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductController(ApplicationDbContext db, IWebHostEnvironment env)
    { _db = db; _env = env; }

    // GET: /Admin/Product/Index
    public async Task<IActionResult> Index()
        => View(await _db.Products
            .Include(p => p.Category)
            .OrderBy(p => p.CategoryId).ToListAsync());

    // GET: /Admin/Product/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(
            await _db.Categories.ToListAsync(), "Id", "Name");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (imageFile != null) product.ImageUrl = await SaveImageAsync(imageFile);
        if (ModelState.IsValid)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(
            await _db.Categories.ToListAsync(), "Id", "Name");
        return View(product);
    }

    // GET: /Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return NotFound();
        ViewBag.Categories = new SelectList(
            await _db.Categories.ToListAsync(), "Id", "Name", p.CategoryId);
        return View(p);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
    {
        if (imageFile != null) product.ImageUrl = await SaveImageAsync(imageFile);
        if (ModelState.IsValid)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Cập nhật thành công!";
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(
            await _db.Categories.ToListAsync(), "Id", "Name");
        return View(product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p != null) { _db.Products.Remove(p); await _db.SaveChangesAsync(); }
        TempData["Success"] = "Đã xóa sản phẩm!";
        return RedirectToAction("Index");
    }

    private async Task<string> SaveImageAsync(IFormFile file)
    {
        var folder = Path.Combine(_env.WebRootPath, "images", "products");
        Directory.CreateDirectory(folder);
        var name = Guid.NewGuid() + Path.GetExtension(file.FileName);
        await using var s = new FileStream(Path.Combine(folder, name), FileMode.Create);
        await file.CopyToAsync(s);
        return "/images/products/" + name;
    }
}
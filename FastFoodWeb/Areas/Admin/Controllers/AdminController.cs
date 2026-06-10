using FastFoodWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminController(ApplicationDbContext db) => _db = db;

    // GET: /Admin/Admin/Index — Dashboard
    public async Task<IActionResult> Index()
    {
        ViewBag.TotalProducts = await _db.Products.CountAsync();
        ViewBag.TotalCategories = await _db.Categories.CountAsync();
        ViewBag.TotalOrders = await _db.Orders.CountAsync();
        ViewBag.PendingOrders = await _db.Orders
            .CountAsync(o => o.Status == "Chờ xác nhận");
        ViewBag.TotalRevenue = await _db.Orders
            .Where(o => o.Status == "Hoàn thành")
            .SumAsync(o => (decimal?)o.TotalPrice) ?? 0;
        ViewBag.RecentOrders = await _db.Orders
            .OrderByDescending(o => o.OrderDate).Take(5).ToListAsync();
        return View();
    }

    // GET: /Admin/Admin/Orders
    public async Task<IActionResult> Orders()
        => View(await _db.Orders
            .Include(o => o.OrderDetails)
            .OrderByDescending(o => o.OrderDate).ToListAsync());

    // POST: /Admin/Admin/UpdateOrderStatus
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(int id, string status)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order != null)
        {
            order.Status = status;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Cập nhật trạng thái thành công!";
        }
        return RedirectToAction("Orders");
    }

    // GET: /Admin/Admin/Categories
    public async Task<IActionResult> Categories()
        => View(await _db.Categories.OrderBy(c => c.Name).ToListAsync());

    // POST: /Admin/Admin/CreateCategory
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(string name, string? description)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            _db.Categories.Add(new FastFoodWeb.Models.Category
            {
                Name = name.Trim(),
                Description = description?.Trim(),
                ImageUrl = string.Empty
            });
            await _db.SaveChangesAsync();
            TempData["Success"] = "Thêm danh mục thành công!";
        }
        return RedirectToAction("Categories");
    }

    // POST: /Admin/Admin/DeleteCategory
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category != null)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Đã xóa danh mục!";
        }
        return RedirectToAction("Categories");
    }
}
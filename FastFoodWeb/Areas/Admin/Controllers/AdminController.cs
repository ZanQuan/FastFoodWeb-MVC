using FastFoodWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FastFoodWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AdminController(
        ApplicationDbContext db,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: /Admin/Admin/Index — Dashboard
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin,NhanVien")]
    public async Task<IActionResult> Orders()
        => View(await _db.Orders
            .Include(o => o.OrderDetails)
            .OrderByDescending(o => o.OrderDate).ToListAsync());

    // POST: /Admin/Admin/UpdateOrderStatus
    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,NhanVien")]
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Categories()
        => View(await _db.Categories.OrderBy(c => c.Name).ToListAsync());

    // POST: /Admin/Admin/CreateCategory
    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    // MANAGE USERS 
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Users()
    {
        var users = _userManager.Users.ToList();
        // Lấy role của từng user
        var userRoles = new Dictionary<string, IList<string>>();
        foreach (var u in users)
            userRoles[u.Id] = await _userManager.GetRolesAsync(u);

        ViewBag.UserRoles = userRoles;
        ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        return View(users);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRole(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Xóa toàn bộ role cũ rồi gán role mới
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, newRole);

        TempData["Success"] = $"Đã cập nhật quyền cho {user.Email}!";
        return RedirectToAction("Users");
    }
}
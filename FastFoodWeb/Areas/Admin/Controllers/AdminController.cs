using FastFoodWeb.Hubs;
using Microsoft.AspNetCore.SignalR;
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
    private readonly IHubContext<OrderHub> _hub;
    public AdminController(
        ApplicationDbContext db,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IHubContext<OrderHub> hub)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _hub = hub;
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

    // 1. Nhân viên xác nhận đơn → bếp bắt đầu làm
    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,NhanVien")]
    public async Task<IActionResult> ConfirmOrder(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order != null && order.Status == "Chờ xác nhận")
        {
            order.Status = "Đang xử lý";
            await _db.SaveChangesAsync();
            // Thông báo cho khách hàng
            await _hub.Clients.Group($"user-{order.UserId}")
                .SendAsync("StatusChanged", "Đang xử lý");
            TempData["Success"] = $"Đã xác nhận đơn #{id}!";
        }
        return RedirectToAction("Orders");
    }

    // 2. Bếp xong → giao hàng
    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,NhanVien")]
    public async Task<IActionResult> MarkDelivering(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order != null && order.Status == "Đang xử lý")
        {
            order.Status = "Đang giao";
            await _db.SaveChangesAsync();
            await _hub.Clients.Group($"user-{order.UserId}")
                .SendAsync("StatusChanged", "Đang giao");
            TempData["Success"] = $"Đơn #{id} đang được giao!";
        }
        return RedirectToAction("Orders");
    }

    // 3. Hủy đơn
    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,NhanVien")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order != null && order.Status != "Hoàn thành")
        {
            order.Status = "Hủy";
            await _db.SaveChangesAsync();
            await _hub.Clients.Group($"user-{order.UserId}")
                .SendAsync("StatusChanged", "Hủy");
            TempData["Success"] = $"Đã hủy đơn #{id}.";
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
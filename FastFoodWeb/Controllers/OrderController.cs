using FastFoodWeb.Hubs;
using Microsoft.AspNetCore.SignalR;
using FastFoodWeb.Data;
using FastFoodWeb.Helpers;
using FastFoodWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FastFoodWeb.Controllers;

[Authorize]  
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<OrderHub> _hub;

    public OrderController(ApplicationDbContext context, IHubContext<OrderHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    // GET: /Order/Checkout
    public IActionResult Checkout()
    {
        var cart = CartHelper.GetCart(HttpContext.Session);

        // Giỏ trống → quay về giỏ hàng
        if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống!";
            return RedirectToAction("Index", "Cart");
        }

        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.SubTotal);
        return View();
    }

    // POST: /Order/Checkout
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(string address, string? note)
    {
        var cart = CartHelper.GetCart(HttpContext.Session);
        if (!cart.Any())
            return RedirectToAction("Index", "Cart");

        // Lấy Id người dùng đang đăng nhập
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // Tạo đơn hàng + chi tiết trong 1 lần SaveChanges
        var order = new Order
        {
            UserId = userId,
            Address = address,
            Note = note,
            OrderDate = DateTime.Now,
            Status = "Chờ xác nhận",
            TotalPrice = cart.Sum(x => x.SubTotal),
            OrderDetails = cart.Select(c => new OrderDetail
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                UnitPrice = c.Price   // lưu giá tại thời điểm đặt
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        // Gửi thông báo real-time đến nhân viên/admin
        await _hub.Clients.Group("Staff").SendAsync("NewOrder", new
        {
            orderId = order.Id,
            amount = order.TotalPrice.ToString("N0"),
            address = order.Address,
            time = DateTime.Now.ToString("HH:mm"),
            itemCount = order.OrderDetails.Count
        });

        // Xóa giỏ hàng sau khi đặt thành công
        CartHelper.ClearCart(HttpContext.Session);

        TempData["Success"] = "Đặt hàng thành công! Cảm ơn bạn.";
        return RedirectToAction("Track", new { id = order.Id });
    }

    // GET: /Order/Track/5 — trang theo dõi đơn hàng
    public async Task<IActionResult> Track(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        return View(order);
    }

    // GET: /Order/GetStatus/5 — API cho JS polling
    [HttpGet]
    public async Task<IActionResult> GetStatus(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        return Json(new { status = order.Status });
    }

    // POST: /Order/ConfirmReceived/5 — khách xác nhận đã nhận
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmReceived(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        if (order.Status == "Đang giao")
        {
            order.Status = "Hoàn thành";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cảm ơn! Hẹn gặp lại bạn.";
        }
        return RedirectToAction("Track", new { id });
    }

    // GET: /Order/Confirmation/5
    public async Task<IActionResult> Confirmation(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var order = await _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null) return NotFound();
        return View(order);
    }

    // GET: /Order/History
    public async Task<IActionResult> History()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderDetails)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return View(orders);
    }
}
using FastFoodWeb.Hubs;
using Microsoft.AspNetCore.SignalR;
using FastFoodWeb.Data;
using FastFoodWeb.Helpers;
using FastFoodWeb.Models;
using FastFoodWeb.Services;
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
    private readonly VnPayService _vnPay;

    public OrderController(ApplicationDbContext context,
                           IHubContext<OrderHub> hub,
                           VnPayService vnPay)
    {
        _context = context;
        _hub = hub;
        _vnPay = vnPay;
    }

    // ── GET /Order/Checkout ─────────────────────────────────────────────
    public IActionResult Checkout()
    {
        var cart = CartHelper.GetCart(HttpContext.Session);
        if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống!";
            return RedirectToAction("Index", "Cart");
        }
        ViewBag.Cart  = cart;
        ViewBag.Total = cart.Sum(x => x.SubTotal);
        return View();
    }

    // ── POST /Order/Checkout ────────────────────────────────────────────
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(string address, string? note, string paymentMethod)
    {
        var cart = CartHelper.GetCart(HttpContext.Session);
        if (!cart.Any()) return RedirectToAction("Index", "Cart");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var order = new Order
        {
            UserId        = userId,
            Address       = address,
            Note          = note,
            OrderDate     = DateTime.Now,
            Status        = "Chờ xác nhận",
            TotalPrice    = cart.Sum(x => x.SubTotal),
            PaymentMethod = paymentMethod == "VNPay" ? "VNPay" : "COD",
            PaymentStatus = "Chưa thanh toán",
            OrderDetails  = cart.Select(c => new OrderDetail
            {
                ProductId = c.ProductId,
                Quantity  = c.Quantity,
                UnitPrice = c.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Xóa giỏ hàng (kể cả VNPay — đã lưu đơn, sẽ cập nhật khi callback)
        CartHelper.ClearCart(HttpContext.Session);

        if (order.PaymentMethod == "VNPay")
        {
            string orderInfo = $"Thanh toan don hang #{order.Id}";
            string payUrl = _vnPay.CreatePaymentUrl(order.Id, order.TotalPrice, orderInfo);
            return Redirect(payUrl);
        }

        // COD: thông báo nhân viên + chuyển sang trang theo dõi
        await NotifyStaff(order);
        TempData["Success"] = "Đặt hàng thành công! Cảm ơn bạn.";
        return RedirectToAction("Track", new { id = order.Id });
    }

    // ── GET /Order/VnPayReturn  (VNPay callback) ─────────────────────────
    [AllowAnonymous]
    public async Task<IActionResult> VnPayReturn()
    {
        var (isValid, isPaid, txnRef, responseCode) =
            _vnPay.ValidateReturn(Request.Query);

        // txnRef = "orderId_timestamp"
        if (!int.TryParse(txnRef.Split('_')[0], out int orderId))
            return BadRequest("Mã đơn hàng không hợp lệ.");

        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return NotFound();

        if (isPaid)
        {
            order.PaymentStatus = "Đã thanh toán";
            order.VnPayTxnRef   = txnRef;
            await _context.SaveChangesAsync();
            await NotifyStaff(order);

            TempData["Success"] = "Thanh toán thành công! Cảm ơn bạn.";
            return RedirectToAction("Track", new { id = orderId });
        }
        else
        {
            // Thanh toán thất bại → hủy đơn
            order.PaymentStatus = "Thất bại";
            order.Status        = "Hủy";
            order.VnPayTxnRef   = txnRef;
            await _context.SaveChangesAsync();

            TempData["Error"] = $"Thanh toán thất bại (mã lỗi: {responseCode}). Đơn hàng đã bị hủy.";
            return RedirectToAction("PaymentFailed", new { id = orderId });
        }
    }

    // ── GET /Order/PaymentFailed ─────────────────────────────────────────
    public async Task<IActionResult> PaymentFailed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order  = await _context.Orders
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        return View(order);
    }

    // ── GET /Order/Track/5 ──────────────────────────────────────────────
    public async Task<IActionResult> Track(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order  = await _context.Orders
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        return View(order);
    }

    // ── GET /Order/GetStatus/5 ───────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> GetStatus(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order  = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        return Json(new { status = order.Status });
    }

    // ── POST /Order/ConfirmReceived/5 ────────────────────────────────────
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmReceived(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order  = await _context.Orders
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

    // ── GET /Order/Confirmation/5 ────────────────────────────────────────
    public async Task<IActionResult> Confirmation(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order  = await _context.Orders
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        return View(order);
    }

    // ── GET /Order/History ───────────────────────────────────────────────
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

    // ── PRIVATE HELPER ───────────────────────────────────────────────────
    private async Task NotifyStaff(Order order)
    {
        await _hub.Clients.Group("Staff").SendAsync("NewOrder", new
        {
            orderId   = order.Id,
            amount    = order.TotalPrice.ToString("N0"),
            address   = order.Address,
            time      = DateTime.Now.ToString("HH:mm"),
            itemCount = order.OrderDetails.Count,
            payment   = order.PaymentMethod
        });
    }
}

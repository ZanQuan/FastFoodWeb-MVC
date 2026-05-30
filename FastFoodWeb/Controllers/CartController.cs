using FastFoodWeb.Models;
using FastFoodWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FastFoodWeb.Controllers;

public class CartController : Controller
{
    private const string CartKey = "Cart";
    private readonly IProductRepository _productRepo;

    public CartController(IProductRepository productRepo)
        => _productRepo = productRepo;

    // Lấy giỏ hàng từ Session
    private List<CartItem> GetCart()
    {
        var json = HttpContext.Session.GetString(CartKey);
        return json == null
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(json)!;
    }

    // Lưu giỏ hàng vào Session
    private void SaveCart(List<CartItem> cart)
        => HttpContext.Session.SetString(CartKey,
            JsonSerializer.Serialize(cart));

    // GET: /Cart
    public IActionResult Index() => View(GetCart());

    // POST: /Cart/Add
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = await _productRepo.GetByIdAsync(productId);
        if (product == null) return NotFound();

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
            item.Quantity += quantity;
        else
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity
            });

        SaveCart(cart);
        TempData["Success"] = "Đã thêm vào giỏ hàng!";
        return RedirectToAction("Index");
    }

    // POST: /Cart/Remove
    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(c => c.ProductId == productId);
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // POST: /Cart/Update
    [HttpPost]
    public IActionResult Update(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0) cart.Remove(item);
            else item.Quantity = quantity;
        }
        SaveCart(cart);
        return RedirectToAction("Index");
    }
}
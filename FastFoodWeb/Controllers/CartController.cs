using FastFoodWeb.Helpers;
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

    // GET: /Cart
    public IActionResult Index()
        => View(CartHelper.GetCart(HttpContext.Session));

    // POST: /Cart/Add
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = await _productRepo.GetByIdAsync(productId);
        if (product == null) return NotFound();

        var cart = CartHelper.GetCart(HttpContext.Session);
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

        CartHelper.SaveCart(HttpContext.Session, cart);
        TempData["Success"] = "Đã thêm vào giỏ hàng!";
        return RedirectToAction("Index");
    }

    // POST: /Cart/Remove
    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = CartHelper.GetCart(HttpContext.Session);
        cart.RemoveAll(c => c.ProductId == productId);
        CartHelper.SaveCart(HttpContext.Session, cart);
        return RedirectToAction("Index");
    }

    // POST: /Cart/Update
    [HttpPost]
    public IActionResult Update(int productId, int quantity)
    {
        var cart = CartHelper.GetCart(HttpContext.Session);
        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0) cart.Remove(item);
            else item.Quantity = quantity;
        }
        CartHelper.SaveCart(HttpContext.Session, cart);
        return RedirectToAction("Index");
    }
}
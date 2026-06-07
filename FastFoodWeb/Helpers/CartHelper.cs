using FastFoodWeb.Models;
using System.Text.Json;

namespace FastFoodWeb.Helpers;

public static class CartHelper
{
    private const string CartKey = "Cart";

    // Lấy giỏ hàng từ Session
    public static List<CartItem> GetCart(ISession session)
    {
        var json = session.GetString(CartKey);
        return json == null
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(json)!;
    }

    // Lưu giỏ hàng vào Session
    public static void SaveCart(ISession session, List<CartItem> cart)
        => session.SetString(CartKey, JsonSerializer.Serialize(cart));

    // Xóa giỏ hàng khỏi Session
    public static void ClearCart(ISession session)
        => session.Remove(CartKey);
}
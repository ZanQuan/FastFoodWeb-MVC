using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodWeb.Models;

public class OrderDetail
{
    public int Id { get; set; }

    // Khóa ngoại → Order
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    // Khóa ngoại → Product
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }

    // Lưu giá tại thời điểm đặt (giá có thể thay đổi sau)
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    // Tính tổng tiền của dòng này
    public decimal SubTotal => Quantity * UnitPrice;
}
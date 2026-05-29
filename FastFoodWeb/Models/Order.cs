using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodWeb.Models;

public class Order
{
    public int Id { get; set; }

    // Liên kết với user đặt hàng (Identity dùng string Id)
    public string UserId { get; set; } = "";

    [Display(Name = "Ngày đặt")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Tổng tiền")]
    public decimal TotalPrice { get; set; }

    // Trạng thái: "Chờ xác nhận" | "Đang giao" | "Hoàn thành" | "Hủy"
    [Display(Name = "Trạng thái")]
    public string Status { get; set; } = "Chờ xác nhận";

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
    [Display(Name = "Địa chỉ giao")]
    public string Address { get; set; } = "";

    [Display(Name = "Ghi chú")]
    public string? Note { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = [];
}
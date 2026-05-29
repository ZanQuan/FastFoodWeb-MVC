using System.ComponentModel.DataAnnotations;

namespace FastFoodWeb.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên danh mục không được bỏ trống")]
    [StringLength(100)]
    [Display(Name = "Tên danh mục")]
    public string Name { get; set; } = "";

    [Display(Name = "Hình ảnh")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    // Navigation: 1 danh mục có nhiều sản phẩm
    public ICollection<Product> Products { get; set; } = [];
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodWeb.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
    [StringLength(200)]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = "";

    [Required]
    [Range(1000, 10000000, ErrorMessage = "Giá phải từ 1.000đ đến 10.000.000đ")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Giá bán")]
    public decimal Price { get; set; }

    [Display(Name = "Hình ảnh")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Tồn kho")]
    public int Stock { get; set; } = 100;

    [Display(Name = "Còn bán")]
    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    // Khóa ngoại → Category
    [Required]
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
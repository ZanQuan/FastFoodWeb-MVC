using FastFoodWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FastFoodWeb.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // Khai báo bảng trong DB
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed danh mục
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Burger" },
            new Category { Id = 2, Name = "Gà rán" },
            new Category { Id = 3, Name = "Pizza" },
            new Category { Id = 4, Name = "Đồ uống" },
            new Category { Id = 5, Name = "Tráng miệng" }
        );

        // Seed sản phẩm mẫu
        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Smash Burger Bò", Price = 149000, CategoryId = 1, IsFeatured = true },
            new Product { Id = 2, Name = "Burger Gà Giòn", Price = 129000, CategoryId = 1 },
            new Product { Id = 3, Name = "Gà Rán 2 Miếng", Price = 99000, CategoryId = 2, IsFeatured = true },
            new Product { Id = 4, Name = "Pizza Hải Sản", Price = 199000, CategoryId = 3 },
            new Product { Id = 5, Name = "Coca Cola", Price = 25000, CategoryId = 4 },
            new Product { Id = 6, Name = "Bánh Lava Nutella", Price = 89000, CategoryId = 5, IsFeatured = true }
        );
    }
}
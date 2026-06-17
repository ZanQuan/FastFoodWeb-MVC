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

           // --- Burger ---
           new Product
           {
               Id = 1,
               Name = "Smash Burger Bò",
               Price = 149000,
               CategoryId = 1,
               Description = "Burger bò Úc xay thủ công, sốt đặc biệt, rau tươi giòn.",
               ImageUrl = "/images/smartburger.jpg",
               Stock = 100,
               IsAvailable = true,
               IsFeatured = true
           },
           new Product
           {
               Id = 2,
               Name = "Burger Gà Giòn",
               Price = 129000,
               CategoryId = 1,
               Description = "Đùi gà chiên giòn, sốt mayo tỏi, dưa leo tươi.",
               ImageUrl = "/images/burgergagion.jpg",
               Stock = 100,
               IsAvailable = true,
               IsFeatured = false
           },

           // --- Gà rán ---
           new Product
           {
               Id = 3,
               Name = "Gà Rán 2 Miếng",
               Price = 99000,
               CategoryId = 2,
               Description = "Gà rán vàng giòn theo công thức gia truyền, kèm sốt chua ngọt.",
               ImageUrl = "/images/garan2mieng.jpg",
               Stock = 100,
               IsAvailable = true,
               IsFeatured = true
           },
           new Product
           {
               Id = 4,
               Name = "Cánh Gà Chiên Mắm",
               Price = 79000,
               CategoryId = 2,
               Description = "Cánh gà chiên ngập dầu, phủ sốt mắm tỏi ớt đậm đà.",
               ImageUrl = "/images/canhgachienmam.png",
               Stock = 80,
               IsAvailable = true,
               IsFeatured = false
           },

           // --- Pizza ---
           new Product
           {
               Id = 5,
               Name = "Pizza Hải Sản",
               Price = 199000,
               CategoryId = 3,
               Description = "Pizza đế mỏng, topping tôm mực bạch tuộc, phô mai kéo sợi.",
               ImageUrl = "/images/pizzahaisan.png",
               Stock = 50,
               IsAvailable = true,
               IsFeatured = false
           },
           new Product
           {
               Id = 6,
               Name = "Pizza Bò BBQ",
               Price = 189000,
               CategoryId = 3,
               Description = "Thịt bò nướng BBQ, hành tây, ớt chuông, sốt đặc biệt.",
               ImageUrl = "/images/pizzabo.jpg",
               Stock = 50,
               IsAvailable = true,
               IsFeatured = false
           },

           // --- Đồ uống ---
           new Product
           {
               Id = 7,
               Name = "Coca Cola",
               Price = 25000,
               CategoryId = 4,
               Description = "Coca Cola lon 330ml, uống lạnh cực đã.",
               ImageUrl = "/images/cocacola.jpg",
               Stock = 200,
               IsAvailable = true,
               IsFeatured = false
           },
           new Product
           {
               Id = 8,
               Name = "Khoai Tây Chiên Vừa",
               Price = 39000,
               CategoryId = 4,
               Description = "Khoai tây cắt sợi chiên vàng giòn, muối tiêu, chấm tương ớt.",
               ImageUrl = "/images/khoaitay.jpg",
               Stock = 150,
               IsAvailable = true,
               IsFeatured = false
           },

           // --- Tráng miệng ---
           new Product
           {
               Id = 9,
               Name = "Bánh Lava Nutella",
               Price = 89000,
               CategoryId = 5,
               Description = "Bánh chocolate nóng chảy bên trong, ăn kèm kem vani.",
               ImageUrl = "/images/banhlava.jpg",
               Stock = 60,
               IsAvailable = true,
               IsFeatured = true
           },
           new Product
           {
               Id = 10,
               Name = "Wrap Gà Teriyaki",
               Price = 109000,
               CategoryId = 1,
               Description = "Bánh mì cuộn gà nướng teriyaki, rau xà lách, cà rốt bào.",
               ImageUrl = "/images/wrapga.jpg",
               Stock = 80,
               IsAvailable = true,
               IsFeatured = false
           },
           new Product
           {
               Id = 11,
               Name = "Double Cheese Burger",
               Price = 169000,
               CategoryId = 1,
               Description = "Hai lớp thịt bò, hai lớp phô mai Cheddar tan chảy, dưa leo muối.",
               ImageUrl = "/images/doublecheese.jpg",
               Stock = 80,
               IsAvailable = true,
               IsFeatured = true
           },
            new Product
            {
                Id = 12,
                Name = "Burger Tôm Tempura",
                Price = 139000,
                CategoryId = 1,
                Description = "Tôm sú tẩm bột tempura chiên giòn, sốt Thousand Island, xà lách.",
                ImageUrl = "/images/bugertom.jpg",
                Stock = 70,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 13,
                Name = "Mushroom Swiss Burger",
                Price = 159000,
                CategoryId = 1,
                Description = "Burger bò kèm nấm xào bơ tỏi và phô mai Swiss béo ngậy.",
                ImageUrl = "/images/mushroomswissburger.jpg",
                Stock = 60,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 14,
                Name = "Gà Rán Phô Mai",
                Price = 115000,
                CategoryId = 2,
                Description = "Miếng gà rán bọc lớp phô mai cheddar chảy vàng bên ngoài.",
                ImageUrl = "/images/garanphomai.jpg",
                Stock = 90,
                IsAvailable = true,
                IsFeatured = true
            },
            new Product
            {
                Id = 15,
                Name = "Cánh Gà Sốt Cam",
                Price = 89000,
                CategoryId = 2,
                Description = "Cánh gà chiên giòn sốt cam mật ong chua ngọt, thơm lừng.",
                ImageUrl = "/images/canhgasotcam.jpg",
                Stock = 75,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 16,
                Name = "Đùi Gà Nướng BBQ",
                Price = 95000,
                CategoryId = 2,
                Description = "Đùi gà ướp BBQ nướng than hoa, da giòn, thịt mềm thấm gia vị.",
                ImageUrl = "/images/ganuongbbq.jpg",
                Stock = 60,
                IsAvailable = true,
                IsFeatured = false
            },
            
            new Product
            {
                Id = 17,
                Name = "Pizza 4 Phô Mai",
                Price = 219000,
                CategoryId = 3,
                Description = "Bốn loại phô mai Mozzarella, Cheddar, Parmesan, Gorgonzola tan chảy.",
                ImageUrl = "/images/pizza4phomai.jpg",
                Stock = 40,
                IsAvailable = true,
                IsFeatured = true
            },
            new Product
            {
                Id = 18,
                Name = "Pizza Gà Nấm",
                Price = 179000,
                CategoryId = 3,
                Description = "Ức gà nướng, nấm đông cô, hành tây caramel, sốt kem trắng.",
                ImageUrl = "/images/pizzaganam.jpg",
                Stock = 45,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 19,
                Name = "Pizza Xúc Xích Ý",
                Price = 185000,
                CategoryId = 3,
                Description = "Xúc xích Pepperoni, ớt chuông nhiều màu, olive đen, sốt cà.",
                ImageUrl = "/images/pizzay.png",
                Stock = 45,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 20,
                Name = "Trà Sữa Trân Châu",
                Price = 45000,
                CategoryId = 4,
                Description = "Trà sữa Đài Loan đậm đà, trân châu đen dẻo, đá lạnh.",
                ImageUrl = "/images/trasuachanchau.jpg",
                Stock = 150,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 21,
                Name = "Sinh Tố Dâu",
                Price = 49000,
                CategoryId = 4,
                Description = "Dâu tây tươi xay nhuyễn, sữa tươi không đường, đá bào mịn.",
                ImageUrl = "/images/cocacola.jpg",
                Stock = 100,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 22,
                Name = "Nước Cam Ép",
                Price = 39000,
                CategoryId = 4,
                Description = "Cam Valencia ép tươi nguyên chất, không thêm đường, giàu vitamin C.",
                ImageUrl = "/images/sinhtodau.jpg",
                Stock = 120,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 23,
                Name = "Kem Gelato Dâu",
                Price = 55000,
                CategoryId = 5,
                Description = "Kem Gelato Ý làm từ dâu tươi, vị béo mịn, ít ngọt thanh mát.",
                ImageUrl = "/images/gelatodau.jpg",
                Stock = 80,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 24,
                Name = "Cheesecake Việt Quất",
                Price = 75000,
                CategoryId = 5,
                Description = "Cheesecake New York đế bánh quy giòn, mặt phủ compote việt quất.",
                ImageUrl = "/images/cheesecakevq.jpg",
                Stock = 50,
                IsAvailable = true,
                IsFeatured = false
            },
            new Product
            {
                Id = 25,
                Name = "Bánh Donut Sô Cô La",
                Price = 35000,
                CategoryId = 5,
                Description = "Donut chiên phồng, phủ ganache chocolate đen, rắc sprinkles màu.",
                ImageUrl = "/images/donutsocola.jpg",
                Stock = 100,
                IsAvailable = true,
                IsFeatured = false
            }
       );
    }
}
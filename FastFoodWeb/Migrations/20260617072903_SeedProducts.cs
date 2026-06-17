using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastFoodWeb.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { "Burger bò Úc xay thủ công, sốt đặc biệt, rau tươi giòn.", "/images/burger.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { "Đùi gà chiên giòn, sốt mayo tỏi, dưa leo tươi.", "/images/burger2.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { "Gà rán vàng giòn theo công thức gia truyền, kèm sốt chua ngọt.", "/images/garan.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 2, "Cánh gà chiên ngập dầu, phủ sốt mắm tỏi ớt đậm đà.", "/images/garan.jpg", "Cánh Gà Chiên Mắm", 79000m, 80 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 3, "Pizza đế mỏng, topping tôm mực bạch tuộc, phô mai kéo sợi.", "/images/pizza.jpg", "Pizza Hải Sản", 199000m, 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "IsFeatured", "Name", "Price", "Stock" },
                values: new object[] { 3, "Thịt bò nướng BBQ, hành tây, ớt chuông, sốt đặc biệt.", "/images/pizza.jpg", false, "Pizza Bò BBQ", 189000m, 50 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsAvailable", "IsFeatured", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 7, 4, "Coca Cola lon 330ml, uống lạnh cực đã.", "/images/wrap.jpg", true, false, "Coca Cola", 25000m, 200 },
                    { 8, 4, "Khoai tây cắt sợi chiên vàng giòn, muối tiêu, chấm tương ớt.", "/images/khoaitay.jpg", true, false, "Khoai Tây Chiên Vừa", 39000m, 150 },
                    { 9, 5, "Bánh chocolate nóng chảy bên trong, ăn kèm kem vani.", "/images/trangmieng.jpg", true, true, "Bánh Lava Nutella", 89000m, 60 },
                    { 10, 1, "Bánh mì cuộn gà nướng teriyaki, rau xà lách, cà rốt bào.", "/images/wrap.jpg", true, false, "Wrap Gà Teriyaki", 109000m, 80 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 3, null, null, "Pizza Hải Sản", 199000m, 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 4, null, null, "Coca Cola", 25000m, 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "IsFeatured", "Name", "Price", "Stock" },
                values: new object[] { 5, null, null, true, "Bánh Lava Nutella", 89000m, 100 });
        }
    }
}

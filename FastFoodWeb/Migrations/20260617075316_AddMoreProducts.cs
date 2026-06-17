using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastFoodWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/smartburger.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/burgergagion.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/garan2mieng.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/canhgachienmam.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/pizzahaisan.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/pizzabo.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/images/cocacola.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "/images/banhlava.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "/images/wrapga.jpg");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsAvailable", "IsFeatured", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 11, 1, "Hai lớp thịt bò, hai lớp phô mai Cheddar tan chảy, dưa leo muối.", "/images/doublecheese.jpg", true, true, "Double Cheese Burger", 169000m, 80 },
                    { 12, 1, "Tôm sú tẩm bột tempura chiên giòn, sốt Thousand Island, xà lách.", "/images/burgertom.jpg", true, false, "Burger Tôm Tempura", 139000m, 70 },
                    { 13, 1, "Burger bò kèm nấm xào bơ tỏi và phô mai Swiss béo ngậy.", "/images/mushroomswissburger.jpg", true, false, "Mushroom Swiss Burger", 159000m, 60 },
                    { 14, 2, "Miếng gà rán bọc lớp phô mai cheddar chảy vàng bên ngoài.", "/images/garanphomai.jpg", true, true, "Gà Rán Phô Mai", 115000m, 90 },
                    { 15, 2, "Cánh gà chiên giòn sốt cam mật ong chua ngọt, thơm lừng.", "/images/canhgasotcam.png", true, false, "Cánh Gà Sốt Cam", 89000m, 75 },
                    { 16, 2, "Đùi gà ướp BBQ nướng than hoa, da giòn, thịt mềm thấm gia vị.", "/images/ganuongbbq.jpg", true, false, "Đùi Gà Nướng BBQ", 95000m, 60 },
                    { 17, 3, "Bốn loại phô mai Mozzarella, Cheddar, Parmesan, Gorgonzola tan chảy.", "/images/pizza4phomai.jpg", true, true, "Pizza 4 Phô Mai", 219000m, 40 },
                    { 18, 3, "Ức gà nướng, nấm đông cô, hành tây caramel, sốt kem trắng.", "/images/pizzaganam.png", true, false, "Pizza Gà Nấm", 179000m, 45 },
                    { 19, 3, "Xúc xích Pepperoni, ớt chuông nhiều màu, olive đen, sốt cà.", "/images/pizzay.png", true, false, "Pizza Xúc Xích Ý", 185000m, 45 },
                    { 20, 4, "Trà sữa Đài Loan đậm đà, trân châu đen dẻo, đá lạnh.", "/images/trasuachanchau.jpg", true, false, "Trà Sữa Trân Châu", 45000m, 150 },
                    { 21, 4, "Dâu tây tươi xay nhuyễn, sữa tươi không đường, đá bào mịn.", "/images/cocacola.jpg", true, false, "Sinh Tố Dâu", 49000m, 100 },
                    { 22, 4, "Cam Valencia ép tươi nguyên chất, không thêm đường, giàu vitamin C.", "/images/sinhtodau.jpg", true, false, "Nước Cam Ép", 39000m, 120 },
                    { 23, 5, "Kem Gelato Ý làm từ dâu tươi, vị béo mịn, ít ngọt thanh mát.", "/images/gelatodau.jpg", true, false, "Kem Gelato Dâu", 55000m, 80 },
                    { 24, 5, "Cheesecake New York đế bánh quy giòn, mặt phủ compote việt quất.", "/images/cheesecakevq.jpg", true, false, "Cheesecake Việt Quất", 75000m, 50 },
                    { 25, 5, "Donut chiên phồng, phủ ganache chocolate đen, rắc sprinkles màu.", "/images/donutsocola.jpg", true, false, "Bánh Donut Sô Cô La", 35000m, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/burger.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/burger2.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/garan.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/garan.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/pizza.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/pizza.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/images/wrap.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "/images/trangmieng.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "/images/wrap.jpg");
        }
    }
}

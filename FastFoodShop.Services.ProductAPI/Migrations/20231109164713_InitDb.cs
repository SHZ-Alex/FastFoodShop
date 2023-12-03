using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastFood.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLocalPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryName", "Description", "ImageLocalPath", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Хлеб", " Самса от дяди васи.<br/> Приготовлена в горах Сванетии. 2+1 акция.", null, "https://gotovim-doma.ru/images/recipe/3/50/350f7e02367db6ab8bc271bc7a836495.jpg", "Самса", 15.0 },
                    { 2, "Шаурма", " Та самая шаурма на средном.<br/> По рецепту НиНо. С тем же соусом, та самая.", null, "https://media-cdn.tripadvisor.com/media/photo-s/16/50/30/86/caption.jpg", "Шаурма", 19.989999999999998 },
                    { 3, "Дессерт", " Просто топ. Ой, картина не та", null, "https://siapress.ru/images/news/main/118312-v-hanti-mansiyske-nashli-samiy-gigantskiy-muraveynik-v-rf.jpg", "Муравейник", 10.99 },
                    { 4, "Хлеб", " Куплена на втором этаже легендарного магазина в Казахстане.<br/> Жаренная сосиска в тесте. Саше возьмите 2 штуки.", null, "https://cdn.lifehacker.ru/wp-content/uploads/2021/10/Depositphotos_446841358_XL_1633610444-scaled-e1633610551746-1280x640.jpg", "Сосиска в тесте", 250.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}

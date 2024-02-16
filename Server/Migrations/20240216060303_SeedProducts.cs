using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Server.Migrations
{
    public partial class SeedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "LORT kraina książek", "https://krainaksiazek.pl/the_lord_of_the_rings_deluxe_edition-9780544273443.jpg", 19.99m, "Lord of the rings" },
                    { 2, "Tom 1", "https://static.audioteka.com/pl/images/products/marcin-podlewski/glebia-skokowiec-duze.jpg", 9.99m, "Głębia - skokowiec" },
                    { 3, "Tom 2", "https://www.storytel.com/images/640x640/0001986247.jpg", 9.99m, "Glębia - powrót" },
                    { 4, "Tom 3", "https://s.lubimyczytac.pl/upload/books/3872000/3872221/794531-352x500.jpg", 9.99m, "Głębia - napór" },
                    { 5, "Tom 4", "https://fabrykaslow.com.pl/wp/wp-content/uploads/2018/04/PODLEWSKI_Bezkres-x_2D-mala-192x300.jpg", 9.99m, "Głębia - bezkres" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}

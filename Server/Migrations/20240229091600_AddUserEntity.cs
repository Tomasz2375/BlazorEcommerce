using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Server.Migrations
{
    public partial class AddUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productVariants_Products_ProductId",
                table: "productVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_productVariants_ProductTypes_ProductTypeId",
                table: "productVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productVariants",
                table: "productVariants");

            migrationBuilder.RenameTable(
                name: "productVariants",
                newName: "ProductVariants");

            migrationBuilder.RenameIndex(
                name: "IX_productVariants_ProductTypeId",
                table: "ProductVariants",
                newName: "IX_ProductVariants_ProductTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants",
                columns: new[] { "ProductId", "ProductTypeId" });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_ProductTypes_ProductTypeId",
                table: "ProductVariants",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_ProductTypes_ProductTypeId",
                table: "ProductVariants");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants");

            migrationBuilder.RenameTable(
                name: "ProductVariants",
                newName: "productVariants");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariants_ProductTypeId",
                table: "productVariants",
                newName: "IX_productVariants_ProductTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productVariants",
                table: "productVariants",
                columns: new[] { "ProductId", "ProductTypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_productVariants_Products_ProductId",
                table: "productVariants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productVariants_ProductTypes_ProductTypeId",
                table: "productVariants",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

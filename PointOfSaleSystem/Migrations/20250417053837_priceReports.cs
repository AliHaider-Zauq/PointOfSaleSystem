using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PointOfSaleSystem.Migrations
{
    /// <inheritdoc />
    public partial class priceReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "SalePrice");

            migrationBuilder.RenameColumn(
                name: "ProductPrice",
                table: "OrderItems",
                newName: "ProductSalePrice");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductPurchasePrice",
                table: "OrderItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPurchasePrice",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ProductSalePrice",
                table: "OrderItems",
                newName: "ProductPrice");
        }
    }
}

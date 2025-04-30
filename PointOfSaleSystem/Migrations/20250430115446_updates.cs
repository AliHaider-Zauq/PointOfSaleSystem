using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PointOfSaleSystem.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_PurchaseOrderItemId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderItemId",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "ReturnId",
                table: "ReturnOrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnId",
                table: "ReturnOrderItems");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderItemId",
                table: "OrderItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_PurchaseOrderItemId",
                table: "OrderItems",
                column: "PurchaseOrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "OrderItems",
                column: "PurchaseOrderItemId",
                principalTable: "PurchaseOrderItems",
                principalColumn: "Id");
        }
    }
}

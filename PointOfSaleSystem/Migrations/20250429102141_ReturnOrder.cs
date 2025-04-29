using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PointOfSaleSystem.Migrations
{
    /// <inheritdoc />
    public partial class ReturnOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReturnedQuantity",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReturnOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnId = table.Column<int>(type: "integer", nullable: false),
                    ReturnOrderId = table.Column<int>(type: "integer", nullable: false),
                    OrderItemId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderItems_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnOrderItems_ReturnOrders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderItems_OrderItemId",
                table: "ReturnOrderItems",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderItems_ReturnOrderId",
                table: "ReturnOrderItems",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrders_OrderId",
                table: "ReturnOrders",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnOrderItems");

            migrationBuilder.DropTable(
                name: "ReturnOrders");

            migrationBuilder.DropColumn(
                name: "ReturnedQuantity",
                table: "OrderItems");
        }
    }
}

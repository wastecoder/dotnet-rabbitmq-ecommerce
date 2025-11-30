using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMetricsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductMetrics",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    LowStockThreshold = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMetrics", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductSalesStats",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TotalSales = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSalesStats", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "SalesMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalSales = table.Column<int>(type: "integer", nullable: false),
                    ConfirmedSales = table.Column<int>(type: "integer", nullable: false),
                    CanceledSales = table.Column<int>(type: "integer", nullable: false),
                    LastOrderAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAlerts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMetrics_Quantity",
                table: "ProductMetrics",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSalesStats_TotalSales",
                table: "ProductSalesStats",
                column: "TotalSales");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlerts_ProductId",
                table: "StockAlerts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlerts_Timestamp",
                table: "StockAlerts",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMetrics");

            migrationBuilder.DropTable(
                name: "ProductSalesStats");

            migrationBuilder.DropTable(
                name: "SalesMetrics");

            migrationBuilder.DropTable(
                name: "StockAlerts");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CFMS.ReportingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyRevenueReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FranchiseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrders = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRevenueReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopSellingProductReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FranchiseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopSellingProductReports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRevenueReports_FranchiseId_Date",
                table: "DailyRevenueReports",
                columns: new[] { "FranchiseId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_TopSellingProductReports_FranchiseId_Date",
                table: "TopSellingProductReports",
                columns: new[] { "FranchiseId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyRevenueReports");

            migrationBuilder.DropTable(
                name: "TopSellingProductReports");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CFMS.DeliveryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryType = table.Column<int>(type: "int", nullable: false),
                    FranchiseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PickupAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryJobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryJobs_FranchiseId",
                table: "DeliveryJobs",
                column: "FranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryJobs_ReferenceId",
                table: "DeliveryJobs",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryJobs_ShipperId",
                table: "DeliveryJobs",
                column: "ShipperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryJobs");
        }
    }
}

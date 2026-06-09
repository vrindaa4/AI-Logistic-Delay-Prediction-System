using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogisticsAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    EstimatedDeliveryDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "Id", "Carrier", "CreatedAtUtc", "DeliveredAtUtc", "Destination", "EstimatedDeliveryDateUtc", "Origin", "ShipmentNumber", "Status", "TrackingNumber", "Weight" },
                values: new object[,]
                {
                    { 1, "FedEx", new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, "Los Angeles", new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "New York", "SHIP-001", "in-transit", "FDX123456789", 15.5 },
                    { 2, "UPS", new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Miami", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Chicago", "SHIP-002", "delivered", "UPS987654321", 8.3000000000000007 },
                    { 3, "DHL", new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "Boston", new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Seattle", "SHIP-003", "delayed", "DHL456789012", 12.0 },
                    { 4, "FedEx", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, "Delhi", new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Chandigarh", "SHIP-004", "in-transit", "FDX234567890", 20.100000000000001 },
                    { 5, "UPS", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hyderabad", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Pune", "SHIP-005", "in-transit", "UPS345678901", 6.7000000000000002 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipments");
        }
    }
}

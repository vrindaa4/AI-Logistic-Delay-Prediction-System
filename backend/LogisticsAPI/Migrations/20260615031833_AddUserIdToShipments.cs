using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogisticsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToShipments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shipments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Shipments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Shipments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Shipments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Shipments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_UserId",
                table: "Shipments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Users_UserId",
                table: "Shipments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Users_UserId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_UserId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Shipments");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}

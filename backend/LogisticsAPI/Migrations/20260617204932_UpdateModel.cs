using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogisticsAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Users_UserId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ShipmentNumber",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_TrackingNumber",
                table: "Shipments");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "User");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Shipments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "Id", "Carrier", "CreatedAtUtc", "DeliveredAtUtc", "Destination", "EstimatedDeliveryDateUtc", "Origin", "ShipmentNumber", "Status", "TrackingNumber", "UserId", "Weight" },
                values: new object[,]
                {
                    { 1, "FedEx", new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, "Los Angeles", new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "New York", "SHIP-001", "in-transit", "FDX123456789", null, 15.5 },
                    { 2, "UPS", new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Miami", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Chicago", "SHIP-002", "delivered", "UPS987654321", null, 8.3000000000000007 },
                    { 3, "DHL", new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "Boston", new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Seattle", "SHIP-003", "delayed", "DHL456789012", null, 12.0 },
                    { 4, "FedEx", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, "Delhi", new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Chandigarh", "SHIP-004", "in-transit", "FDX234567890", null, 20.100000000000001 },
                    { 5, "UPS", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hyderabad", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Pune", "SHIP-005", "in-transit", "UPS345678901", null, 6.7000000000000002 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Users_UserId",
                table: "Shipments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Users_UserId",
                table: "Shipments");

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
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipmentNumber",
                table: "Shipments",
                column: "ShipmentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_TrackingNumber",
                table: "Shipments",
                column: "TrackingNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Users_UserId",
                table: "Shipments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

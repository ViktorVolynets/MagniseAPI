using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagniseAPI.Migrations
{
    /// <inheritdoc />
    public partial class removedtestdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("24810dfc-2d94-4cc7-aab5-cdf98b83f0c9"));

            migrationBuilder.DeleteData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"));

            migrationBuilder.DeleteData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"));

            migrationBuilder.DeleteData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "Symbol" },
                values: new object[,]
                {
                    { new Guid("24810dfc-2d94-4cc7-aab5-cdf98b83f0c9"), "GB/EUR" },
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "USD/EUR" },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "UAH/USD" }
                });

            migrationBuilder.InsertData(
                table: "Prices",
                columns: new[] { "Id", "AssetId", "Price", "UpdateTime" },
                values: new object[,]
                {
                    { new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), 12.75m, new DateTime(2024, 6, 22, 1, 39, 24, 708, DateTimeKind.Local).AddTicks(1182) },
                    { new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), 679.337m, new DateTime(2024, 6, 22, 1, 39, 24, 708, DateTimeKind.Local).AddTicks(1254) },
                    { new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), 29.44m, new DateTime(2024, 6, 22, 1, 39, 24, 708, DateTimeKind.Local).AddTicks(1250) }
                });
        }
    }
}

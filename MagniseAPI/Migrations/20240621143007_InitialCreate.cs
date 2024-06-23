using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagniseAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "Name" },
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
                    { new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), 12.75m, new DateTime(2024, 6, 21, 17, 30, 5, 604, DateTimeKind.Local).AddTicks(8930) },
                    { new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), 679.337m, new DateTime(2024, 6, 21, 17, 30, 5, 604, DateTimeKind.Local).AddTicks(9115) },
                    { new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), 29.44m, new DateTime(2024, 6, 21, 17, 30, 5, 604, DateTimeKind.Local).AddTicks(9093) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Name",
                table: "Assets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_AssetId",
                table: "Prices",
                column: "AssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}

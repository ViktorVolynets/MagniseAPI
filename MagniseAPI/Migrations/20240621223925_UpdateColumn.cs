using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagniseAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Assets",
                newName: "Symbol");

            migrationBuilder.RenameIndex(
                name: "IX_Assets_Name",
                table: "Assets",
                newName: "IX_Assets_Symbol");

            migrationBuilder.UpdateData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
                column: "UpdateTime",
                value: new DateTime(2024, 6, 22, 1, 39, 24, 708, DateTimeKind.Local).AddTicks(1182));

            migrationBuilder.UpdateData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"),
                column: "UpdateTime",
                value: new DateTime(2024, 6, 22, 1, 39, 24, 708, DateTimeKind.Local).AddTicks(1254));

            migrationBuilder.UpdateData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"),
                column: "UpdateTime",
                value: new DateTime(2024, 6, 22, 1, 39, 24, 708, DateTimeKind.Local).AddTicks(1250));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Symbol",
                table: "Assets",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Assets_Symbol",
                table: "Assets",
                newName: "IX_Assets_Name");

            migrationBuilder.UpdateData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
                column: "UpdateTime",
                value: new DateTime(2024, 6, 21, 17, 30, 5, 604, DateTimeKind.Local).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"),
                column: "UpdateTime",
                value: new DateTime(2024, 6, 21, 17, 30, 5, 604, DateTimeKind.Local).AddTicks(9115));

            migrationBuilder.UpdateData(
                table: "Prices",
                keyColumn: "Id",
                keyValue: new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"),
                column: "UpdateTime",
                value: new DateTime(2024, 6, 21, 17, 30, 5, 604, DateTimeKind.Local).AddTicks(9093));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgizDisSagligiTakip.Data.Migrations
{
    /// <inheritdoc />
    public partial class OneriSeedDataFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 1,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 2,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 3,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 4,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 5,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 6,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 7,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 8,
                column: "OlusturmaTarihi",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 1,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(7915));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 2,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8114));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 3,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8115));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 4,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8116));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 5,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8117));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 6,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8118));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 7,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8119));

            migrationBuilder.UpdateData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 8,
                column: "OlusturmaTarihi",
                value: new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8120));
        }
    }
}
